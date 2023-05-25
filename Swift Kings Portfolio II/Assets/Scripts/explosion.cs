using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int dmg;
    [SerializeField] int pushbackAmount;
    [SerializeField] Rigidbody rb;
    private void Start()
    {
        Destroy(gameObject,.75f);
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
    }
}
