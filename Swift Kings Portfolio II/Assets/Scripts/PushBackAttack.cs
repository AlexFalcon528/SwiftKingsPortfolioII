using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBackAttack : MonoBehaviour
{
    [SerializeField] int pushbackAmount;
    [Range(0, 100)] [SerializeField] int dmg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag=="Enemy")
        {
           
            IPhysics physicsable = other.GetComponent<IPhysics>();
            if (physicsable != null)
            {
                Vector3 dir = other.transform.position - gameManager.instance.player.transform.position;
                physicsable.TakePushBack(dir * pushbackAmount);
            }
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(dmg);
            }
            
        }
    }
}
