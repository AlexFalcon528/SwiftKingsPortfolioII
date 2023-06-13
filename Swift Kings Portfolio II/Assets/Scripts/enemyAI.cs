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
    [SerializeField] AudioSource aud;

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
    [SerializeField] int pointsWorth;

    [Header("\n-----Enemy Weapon------")]
    [SerializeField] GameObject projectile;
    [Range(.1f, 3f)] [SerializeField] float fireRate;


    [Header("----- Audio -----")]
    
    [SerializeField] AudioClip[] audDamage;
    [SerializeField] [Range(0, 1)] float audDamageVol;
    [SerializeField] AudioClip audDeath;
    [SerializeField] [Range(0, 1)] float audDeathVol;

    bool isShooting;
    bool playerInRange;
    bool isRetreating;
    Vector3 startingPos;
    float angleToPlayer;
    Vector3 playerDir;
    Color colorOrig;
    bool destinationChosen;
    float stoppingDistOrig;
    float speed;
    float retreatDistance;
    int viewConeOrig;
    int difficultyScaling;
    void Start()
    {
        difficultyScaling = (gameManager.instance.difficulty / 2);
        hp *= difficultyScaling;
        fireRate /= difficultyScaling;
        startingPos = transform.position;
        colorOrig = model.material.color;
        
        stoppingDistOrig = agent.stoppingDistance;
        retreatDistance = stoppingDistOrig - 3;
        aud = gameManager.instance.player.GetComponent<AudioSource>();
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
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, 0, playerDir.z), transform.forward);
        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);
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
                        agent.SetDestination(gameManager.instance.player.transform.position);
                    }

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        FacePlayer();
                    }
                    if (!isShooting && angleToPlayer <= shootAngle)
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
        
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
    IEnumerator Fade()
    {
        model.material.SetOverrideTag("RenderType", "Transparent");
        model.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        model.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        model.material.SetInt("_ZWrite", 0);
        model.material.DisableKeyword("_ALPHATEST_ON");
        model.material.EnableKeyword("_ALPHABLEND_ON");
        model.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        model.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        for (float fade =1f;fade>=-0.05f;fade-=.0025f)
        {
            Color c = model.material.color;
            c.a = fade;
            model.material.color = c;
           yield return new WaitForSeconds(.05f);
            transform.position += Vector3.down * 0.0025f;
        }
        Vector3 inground = new Vector3(transform.position.x, -1, transform.position.z);
        
        StopAllCoroutines();
        Destroy(gameObject);
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0)
        {
            aud.PlayOneShot(audDeath, audDeathVol);
            gameManager.instance.UpdateGameGoal(-1);
            gameManager.instance.currentScore += pointsWorth;
            gameManager.instance.points += pointsWorth;
            gameManager.instance.currPoints.text = $"{gameManager.instance.points}";
            anim.SetBool("Dead", true);
            agent.enabled = false;
            StopAllCoroutines();
            GetComponent<CapsuleCollider>().enabled = false;
            StartCoroutine(Fade());
            
                
            
        }
        else
        {
            anim.SetTrigger("Damage");
            aud.PlayOneShot(audDamage[Random.Range(0, audDamage.Length)], audDamageVol);
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
        agent.SetDestination(gameManager.instance.player.transform.position);
        isRetreating = false;//stops the retreat
    }
}
