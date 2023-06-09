using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveProjectile : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int dmg;
    [Range(0, 100)] [SerializeField] int speed;
    [SerializeField] int pushbackAmount;
    [SerializeField] Rigidbody rb;
    [SerializeField] int timer;
    [SerializeField] GameObject particleExplosion;
    bool hit = false;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
        rb.velocity = (gameManager.instance.pScript.futurePos.transform.position - rb.position).normalized * speed;
    }
   
    private void OnTriggerEnter(Collider other)
    {
        if (!hit)
        {
            hit = true;
            IPhysics physicsable = other.GetComponent<IPhysics>();
            if (physicsable != null)
            {
                Vector3 dir = other.transform.position - transform.position;
                physicsable.TakePushBack(dir * pushbackAmount);
            }
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(dmg);
            }
            Instantiate(particleExplosion, rb.position, rb.rotation);
            Destroy(gameObject);
        }
    }
}
