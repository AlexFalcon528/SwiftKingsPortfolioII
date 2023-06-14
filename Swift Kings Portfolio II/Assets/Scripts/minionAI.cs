using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class minionAI : MonoBehaviour,IDamage
    
{   [Header("~~~~~~minion components~~~~~~~~")]
    [SerializeField] GameObject projectile;
    [SerializeField] Renderer model;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Animator anim;
    [SerializeField] GameObject particleExplosion;
    [SerializeField] GameObject floatingDmgText;
    [Header("\n~~~~~Minion Stats~~~~~")]
    [Range(1,100)] [SerializeField] int hp;
    [SerializeField] int turnSpeed;
    [Range(.1f, 3f)] [SerializeField] float fireRate;
    [SerializeField] float animTranSpeed;
    [SerializeField] int pointsWorth;

    bool isShooting;
    bool playerInRange;
    Vector3 playerDir;
    Vector3 playerFutureDir;
    Color colorOrig;
    float speed;
    // Start is called before the first frame update
    void Start()
    {
        colorOrig = model.material.color;
        gameManager.instance.UpdateMinionsCounter(+1);
        
    }
    
    // Update is called once per frame
    void Update()
    {
        playerDir = (gameManager.instance.player.transform.position - transform.position).normalized;
        playerFutureDir = (gameManager.instance.player.transform.position - transform.position).normalized;
        agent.SetDestination(gameManager.instance.pScript.transform.position);
        speed = Mathf.Lerp(speed, agent.velocity.normalized.magnitude, Time.deltaTime * animTranSpeed);
        anim.SetFloat("Speed", speed);
        FacePlayer();
        if (!isShooting)
        {
            
            StartCoroutine(Shoot());
        }    
    }

    public void TakeDamage(int dmg)
    {
        hp -= dmg;
       StartCoroutine( DamageColor());
        if (hp <= 0)
        {
            gameManager.instance.UpdateMinionsCounter(-1);
            gameManager.instance.points += pointsWorth;
            gameManager.instance.currentScore += pointsWorth;
            gameManager.instance.currPoints.text = $"{gameManager.instance.points}";
            Instantiate(particleExplosion,transform.position,transform.rotation);
            Destroy(gameObject);
            gameManager.instance.currentScore++;
            StopAllCoroutines();
        }
        else
        {
            anim.SetTrigger("Damage");
            agent.SetDestination(gameManager.instance.player.transform.position);
            
            StartCoroutine(DamageColor());
            StartCoroutine(ShowDmgText(dmg));
        }

    }

    IEnumerator ShowDmgText(int dmgAmt) {
        GameObject dmgText = Instantiate(floatingDmgText, transform.position, Camera.main.transform.rotation, transform);
        floatingText dmg = dmgText.GetComponent<floatingText>();
        dmg.Initiate(dmgAmt);
        yield return null;
    }

    IEnumerator Shoot()
    {

        isShooting = true;
        anim.SetTrigger("Shoot");

        yield return new WaitForSeconds(fireRate);
        isShooting = false;
    }

    public void CreateBullet()
    {
        Instantiate(projectile, transform.position, transform.rotation);
    }

    void FacePlayer()
    {

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
