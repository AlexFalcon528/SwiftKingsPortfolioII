using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int dmg;
    [Range(0, 100)] [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    [SerializeField] int timer;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.takeDamage(dmg);
        }
    }
}
