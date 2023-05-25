using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage, IPhysics
{
    [Header("-----Components------")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [Header("\n-----Enemy Stats------")]
    [Range(1, 100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [SerializeField] int shootAngle;
    [SerializeField]float viewCone;


    [Header("\n-----Enemy Weapon------")]
    [SerializeField] GameObject projectile;
    [Range(.1f, 3f)] [SerializeField] float fireRate;

    bool isShooting;
    bool playerInRange;
   
    float angleToPlayer;
    Vector3 playerDir;
    Color colorOrig;
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.UpdateGameGoal(+1);
        

    }

    // Update is called once per frame
    IEnumerator Shoot()//shooting function
    {
        isShooting = true;
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
        
    }
    void Update()
    {
        
        if (playerInRange&&CanSeePlayer())
        {

        }
    }
    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;//gatting direction of player
        angleToPlayer = Vector3.Angle(transform.forward, playerDir);//angle between enemy sight and player
        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);
        RaycastHit hit;//variable for object hit by ray
        if(Physics.Raycast(headPos.position,playerDir,out hit))//shoots ray in direction of players and returns object hit
        {
            if(hit.collider.CompareTag("Player")&&angleToPlayer<=viewCone)//if no object is blocking sight and if player is in the enemy's view cone 
            { agent.SetDestination(gameManager.instance.player.transform.position);//enemy moves towards player
                if(agent.remainingDistance<=agent.stoppingDistance)
                {
                    FacePlayer();//makes enemy turn to player
                }
                if(!isShooting&&angleToPlayer<=shootAngle)//starts shooting if enemy's aim is close to player
                {
                    StartCoroutine(Shoot());
                }
                return true;
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
        playerInRange = true;
        agent.SetDestination(gameManager.instance.player.transform.position);
       StartCoroutine( DamageColor());
        if (hp <= 0)
        {
            gameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
       
        // allows enemy to notice player when they get shot
    }

    public void TakePushBack(Vector3 dir) {
        agent.velocity += dir;
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
        }
    }
}
