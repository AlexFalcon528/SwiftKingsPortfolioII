using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int dmg;
    [Range(0, 100)] [SerializeField] int speed;
    [SerializeField] float pushbackAmount;
    [SerializeField] Rigidbody rb;
    [SerializeField] int timer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
        rb.velocity = transform.forward * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(dmg);
        }
        IPhysics physicsable = other.GetComponent<IPhysics>();
        if (physicsable != null)
        {
            Vector3 dir = other.transform.position - transform.position;
            physicsable.TakePushBack(dir * pushbackAmount);
        }
       
        Destroy(gameObject);


    }
}
