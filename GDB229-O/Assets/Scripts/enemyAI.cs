using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemyAI : MonoBehaviour, IDamage
{
    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [SerializeField] int hp;
    Color original;
    [Header("----- Enemy Weapon -----")]
    [Range(2, 300)][SerializeField] int shootDist;
    [Range(0.1f, 3)][SerializeField] float shootRate;
    [SerializeField] GameObject bullet;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        original = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);
        if (!isShooting)
        {
            StartCoroutine(shoot());
        }
    }
    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    public void takeDamage(int dmg) 
    {
        hp -= dmg;
        StartCoroutine(flashColor());
        if(hp <= 0 )
        {
            Destroy(gameObject);
        }
    }
    IEnumerator flashColor()
    {
        
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = original;
    }
}
