using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    MeshFilter model;
    MeshRenderer mat;
    // Start is called before the first frame update
    void Start()
    {
        model = gun.model.GetComponent<MeshFilter>();
        mat = gun.model.GetComponent<MeshRenderer>();
        gun.currAmmo = gun.maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.pScript.gunPickup(gun);
            Destroy(gameObject);
        }
    }
}
