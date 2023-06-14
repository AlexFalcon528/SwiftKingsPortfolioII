using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravitymissile : MonoBehaviour
{
    [SerializeField] int pushbackAmount;
    [Range(0, 100)] [SerializeField] int dmg;
    bool isGravitating;
    [SerializeField] Collider col;
    
    // Start is called before the first frame update
    void Start()
    {
       
        
    }
    private void Update()
    {  
        if(!isGravitating)
        {
            StartCoroutine(Gravitate());
        }
    }
    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            IPhysics physicsable = other.GetComponent<IPhysics>();
            if (physicsable != null)
            {
                Vector3 dir = other.transform.position - transform.position;
                physicsable.TakePushBack(dir.normalized * pushbackAmount * -1);
            }
            IDamage damageable = other.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.TakeDamage(dmg);
            }

        }
    }
    IEnumerator Gravitate()
    {
       
            isGravitating = true;
        col.enabled = false;
            yield return new WaitForSeconds(.3f);
        col.enabled = true;
        isGravitating = false;
        
    }
}
