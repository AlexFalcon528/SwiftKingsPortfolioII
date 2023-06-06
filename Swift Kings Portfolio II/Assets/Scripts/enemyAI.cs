using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour,IDamage,IPhysics
{
    [Header("-----Components------")]
    [SerializeField] Renderer model;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;

    [Header("\n-----Enemy Stats------")]
    [Range(1, 100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [SerializeField] int shootAngle;
    [SerializeField] int viewCone;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    [SerializeField] float retreatTime;//how long enemies will retreat for
    [SerializeField] int runAwayDistance;//how far the enmemy will runaway
    [SerializeField] float animTranSpeed;

    [Header("\n-----Enemy Weapon------")]
    [SerializeField] GameObject projectile;
    [Range(.1f, 3f)] [SerializeField] float fireRate;

    bool isShooting;
    bool playerInRange;
    bool isRetreating;
    Vector3 startingPos;
    float angleToPlayer;
    Vector3 playerDir;
    Vector3 playerFutureDir;
    Color colorOrig;
    bool destinationChosen;
    float stoppingDistOrig;
    float speed;
    float retreatDistance;
    int viewConeOrig;
    void Start()
    {
        startingPos = transform.position;
        colorOrig = model.material.color;
        
        stoppingDistOrig = agent.stoppingDistance;
        retreatDistance = stoppingDistOrig - 3;

        viewConeOrig = viewCone;
    }

    // Update is called once per frame
    IEnumerator Shoot()//shooting function
    {
        isShooting = true;
        anim.SetTrigger("Shoot");

        yield return new WaitForSeconds(fireRate);
        isShooting = false;

    }
    public void CreateBullet()
    {
        Instantiate(projectile, shootPos.position, transform.rotation);
    }
    void Update()
    {
        if (agent.isActiveAndEnabled)
        {
            speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTranSpeed);
            anim.SetFloat("Speed", speed);
            if (playerInRange && !CanSeePlayer())
            {

                StartCoroutine(Roam());

            }
            else if (agent.destination != gameManager.instance.player.transform.position)
            {
                StartCoroutine(Roam());
            }
        }
    }
    bool CanSeePlayer()
    {

        playerDir = gameManager.instance.player.transform.position - headPos.position;
        playerFutureDir = gameManager.instance.pScript.futurePos.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, playerDir);
        // Debug.Log(angleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                if (!isRetreating)//makes sure enemy isn't retreating
                {
                    if (agent.remainingDistance <= retreatDistance) //checks to see if agent needs to retreating
                    {
                        StartCoroutine(Retreat(transform.position - (playerDir.normalized * runAwayDistance), retreatTime));//starts retreating away from player = to retreat distance for however long it's scared
                    }
                    else
                    {
                        agent.stoppingDistance = stoppingDistOrig;
                        agent.SetDestination(gameManager.instance.pScript.futurePos.transform.position);
                    }
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        FacePlayer();
                    }
                    if (!isShooting) //&& angleToPlayer <= shootAngle)
                    {
                        StartCoroutine(Shoot());
                    }
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;

        return false;
    
    }
    void FacePlayer() //turn to player function
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerFutureDir.x, 0, playerFutureDir.z));
        //Quaternion rot = Quaternion.LookRotation(new Vector3(gameManager.instance.pScript.futurePos.transform.position.x, 0, gameManager.instance.pScript.futurePos.transform.position.y));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
    public void TakePushBack(Vector3 dir)
    {
        StartCoroutine(push(dir));
    }

    IEnumerator push(Vector3 direction)
    {
        rb.isKinematic = false;
        agent.enabled = false;
        rb.velocity = direction;
        yield return new WaitForSeconds(0.3f);
        rb.isKinematic = true;
        agent.enabled = true;
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {

            StopAllCoroutines();
            gameManager.instance.UpdateGameGoal(-1);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            GetComponent<CapsuleCollider>().enabled = false;
            Destroy(gameObject, 10);
        }
        else
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(gameManager.instance.player.transform.position);
            StartCoroutine(DamageColor());
        }
    }
    IEnumerator DamageColor()//enemy blinks red when they take damage
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        model.material.color = colorOrig;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;//lets enemy know player is in range
        }
    } private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;// lets enemy know player left range
            agent.stoppingDistance = 0;
        }
    }
    IEnumerator Roam()
    {
        if (!destinationChosen && agent.remainingDistance < 0.05f)
        {
            destinationChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamPauseTime);
            destinationChosen = false;

            Vector3 ranPos = Random.insideUnitSphere * roamDistance;
            ranPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(ranPos, out hit, roamDistance, 1);

            agent.SetDestination(hit.position);
        }
    }
    IEnumerator Retreat(Vector3 retreatPos, float retreatTime)//takes in the position to retreat to and for how long
    {
        isRetreating = true;//sets retreating to true
        agent.stoppingDistance = 0;
        agent.SetDestination(retreatPos);//Sets agent position to the desired retreat location

        yield return new WaitForSeconds(retreatTime);//how long the enemy will continue retreating for

        agent.stoppingDistance = stoppingDistOrig;
        agent.SetDestination(gameManager.instance.pScript.transform.position);
        isRetreating = false;//stops the retreat
    }

}
