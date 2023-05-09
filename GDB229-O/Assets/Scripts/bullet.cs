using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] int dmg;
    [SerializeField] int speed;
    [SerializeField] int timer;

    [SerializeField] Rigidbody rb;
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
            damageable.takeDamage(dmg);
        }
        Destroy(gameObject);
    }
}
