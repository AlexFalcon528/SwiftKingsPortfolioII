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
    [Header("\n-----Enemy Stats------")]
    [Range(1, 100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [SerializeField] int shootAngle;
    [SerializeField]float viewCone;
    [SerializeField] int roamDistance;
    [SerializeField] int roamPauseTime;
    [SerializeField] float animTranSpeed;

    [Header("\n-----Enemy Weapon------")]
    [SerializeField] GameObject projectile;
    [Range(.1f, 3f)] [SerializeField] float fireRate;

    bool isShooting;
    bool playerInRange;
    Vector3 startingPos;
    float angleToPlayer;
    Vector3 playerDir;
    Color colorOrig;
    bool destinationChosen;
    float stoppingDistOrig;
    float speed;
    void Start()
    {
        startingPos = transform.position;
        colorOrig = model.material.color;
        
        stoppingDistOrig = agent.stoppingDistance;
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
        angleToPlayer = Vector3.Angle(playerDir, transform.forward);
        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
                destinationChosen = true;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
                if (!isShooting && angleToPlayer <= shootAngle)
                { StartCoroutine(Shoot()); }
                return true;
            }
            else
            {
                agent.stoppingDistance = 0;
            }
        }
        return false;



       
    }
    void FacePlayer() //turn to player function
    {
        
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {

            gameManager.instance.UpdateGameGoal(-1);
            anim.SetBool("Dead", true);
            agent.enabled = false;
            StopAllCoroutines();
            GetComponent<CapsuleCollider>().enabled = false;
        }
        else
        {
            anim.SetTrigger("Damage");
            
            agent.SetDestination(gameManager.instance.player.transform.position);
            playerInRange = true;
            StartCoroutine(DamageColor());
        }
    }
    public void TakePushBack(Vector3 dir)
    {
        agent.velocity += dir;//enemy gets pushed by our shots
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
            agent.stoppingDistance = stoppingDistOrig;
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
}
