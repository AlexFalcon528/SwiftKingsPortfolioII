using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class explosion : MonoBehaviour
{
    [Range(0, 100)] [SerializeField] int dmg;
    [SerializeField] int pushbackAmount;
    private void Start()
    {
        Destroy(gameObject,.75f);
    }
    private void OnTriggerEnter(Collider other)
    {
        IPhysics physicsable = other.GetComponent<IPhysics>();
        if (physicsable != null)
        {
            Vector3 dir = transform.position - other.transform.position;
            physicsable.TakePushBack(dir * pushbackAmount);
        }
        IDamage damageable = other.GetComponent<IDamage>();
        if (damageable != null)
        {
            damageable.TakeDamage(dmg);
        }
    }
}
