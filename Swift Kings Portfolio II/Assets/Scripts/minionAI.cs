using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class minionAI : MonoBehaviour,IDamage
    
{   [Header("~~~~~~minion components~~~~~~~~")]
    [SerializeField] GameObject projectile;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [Header("\n~~~~~Minion Stats~~~~~")]
    [Range(1,100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [Range(.1f, 3f)] [SerializeField] float fireRate;
    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    Color colorOrig;
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.numberOfMinions++;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    void TakeDamage(int dmg)
    {
        hp -= dmg;
        DamageColor();
        if (hp <= 0)
        {
            gameManager.instance.numberOfMinions--;
            Destroy(gameObject);
        }

    }
    IEnumerator Shoot()
    {
        isShooting = true;
        Instantiate(projectile, transform.position, transform.rotation);
        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }
    void FacePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * turnSpeed);
    }
    IEnumerator DamageColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(.1f);
        model.material.color = colorOrig;
    }
}
