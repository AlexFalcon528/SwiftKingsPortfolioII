using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosiveProjectile : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int dmg;
    [Range(0, 100)] [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] int timer;
    [SerializeField] GameObject particleExplosion;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
        rb.velocity = transform.forward * speed;
    }
   
    private void OnCollisionEnter(Collision other)
    {
        IDamage damageable = other.collider.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(dmg);
        }
           
            Instantiate(particleExplosion, rb.position, rb.rotation);
            Destroy(gameObject);
        
    }
}
