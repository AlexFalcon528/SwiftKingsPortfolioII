using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoPickup : MonoBehaviour
{
    [SerializeField] AudioClip pickupSound;
    int orbs;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            orbs = Random.Range(1, 50); // One out of 50 chance to get an orb
            if(orbs == 1)
            {
                orbs = 1;
            }
            else
            {
                orbs = 0;
            }
            gameManager.instance.pScript.ammoPickup(Random.Range(4,15),orbs);
            gameManager.instance.player.GetComponent<AudioSource>().PlayOneShot(pickupSound, audioManager.instance.audSFXVol);
            Destroy(gameObject);
        }
    }
}
