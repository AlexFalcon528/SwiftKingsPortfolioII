using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour,IDamage
{
    [Header("-----Components------")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [Header("-----Enemy Stats------")]
    [Range(1, 100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [SerializeField] int shootAngle;

    [Header("-----Enemy Weapon------")]
    [SerializeField] GameObject projectile;
    [Range(.1f, 3f)] [SerializeField] float fireRate;

    bool isShooting;
    bool playerInRange;
    float viewCone;
    float angleToPlayer;
    Vector3 playerDir;
    Color colorOrig;
    void Start()
    {
        colorOrig = model.material.color;
        

    }

    // Update is called once per frame
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    void Update()
    {
        if(CanSeePlayer())
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
        if(Physics.Raycast(headPos.position,playerDir,out hit))
        {
            if(hit.collider.CompareTag("Player")&&angleToPlayer<=viewCone)
            { agent.SetDestination(gameManager.instance.player.transform.position);
                if(agent.remainingDistance<=agent.stoppingDistance)
                {
                    FacePlayer();
                }
                if(!isShooting&&angleToPlayer<=shootAngle)
                {
                    StartCoroutine(shoot());
                }
                return true;
            }
        }



        return false;
    }
    void FacePlayer()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
}
