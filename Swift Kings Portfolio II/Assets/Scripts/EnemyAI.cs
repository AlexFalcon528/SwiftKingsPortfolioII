using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour, IDamage
{

    [Header("----- Components -----")]
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;

    [Header("----- Enemy Stats -----")]
    [Range(1, 10)] [SerializeField] int HP;

    [Header("----- Enemy Weapon -----")]
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;
    [SerializeField] GameObject bullet;

    bool isShooting;
    Color colorOrig;


    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;

        //gameManager.instance.updateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(gameManager.instance.player.transform.position);

        if (!isShooting)
            StartCoroutine(shoot());
    }

    IEnumerator shoot()
    {

        isShooting = true;

        Instantiate(bullet, transform.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
    }


    public void takeDamage(int amount)
    {
        HP -= amount;

        StartCoroutine(flashColor());
        if (HP <= 0)
        {
           //gameManager.instance.updateGameGoal(-1);
            Destroy(gameObject);
        }
    }


    IEnumerator flashColor()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        model.material.color = colorOrig;
    }

}
