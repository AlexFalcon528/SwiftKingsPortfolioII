using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadeExplosion : MonoBehaviour
{[SerializeField] GameObject ParticleSystem;
    [Range(0, 100)] [SerializeField] int speed;
    [SerializeField] Rigidbody rb;
    void Start()
    {
        
        rb.velocity = transform.forward* speed;
    }
    private void OnTriggerEnter(Collider other)
    {if(!other.CompareTag("Player"))
        Instantiate(ParticleSystem,transform.position,transform.rotation);
        Destroy(gameObject);
    }
}
