using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class necromancerAI : MonoBehaviour,IDamage
{
    [Header("-----Components------")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform minionSpawnPoint;//where minions spawn from
    [SerializeField] Transform headPos;
    [SerializeField] Transform shootPos;

    [Header("\n----Enemy Stats------")]
    [Range(1, 100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [SerializeField] int shootAngle;
    [SerializeField]  float viewCone;
    [Header("\n-----Enemy Weapon------")]
    
    [SerializeField] GameObject minions;
    [SerializeField] float minionSpawnRate;
    [SerializeField] GameObject projectile;
    [Range(.1f, 3)] [SerializeField] float fireRate;
    
    
    bool isShooting;
    bool isSpawning;
    bool playerInRange;
 
    float angleToPlayer;
    Vector3 playerDir;
    Color colorOrig;
    void Start()
    {
        colorOrig = model.material.color;


    }

    // Update is called once per frame
    IEnumerator spawnMinions()
    {if (gameManager.instance.numberOfMinions < gameManager.instance.maxNumberOfMinions)
        {
            isSpawning = true;
            Instantiate(minions, minionSpawnPoint.position, transform.rotation);
            yield return new WaitForSeconds(minionSpawnRate);
            isSpawning = false;
        }
        
    }
  
    void Update()
    {
        if (playerInRange && CanSeePlayer())
        {

        }
    }
    bool CanSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(transform.forward, playerDir);
        Debug.DrawRay(headPos.position, playerDir);
        Debug.Log(angleToPlayer);
        RaycastHit hit;
        if (Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewCone)
            {
                agent.SetDestination(gameManager.instance.player.transform.position);
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    FacePlayer();
                }
                if(!isSpawning)
                {
                    StartCoroutine(spawnMinions());
                }
                
                if (!isShooting && angleToPlayer <= shootAngle)
                {   
                    StartCoroutine(shoot());
                }
                return true;
            }
        }



        return false;
    }
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
    public void TakeDamage(int dmg)
    {
        hp -= dmg;
        if(hp<=0)
        {
            Destroy(gameObject);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
