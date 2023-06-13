using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    MeshFilter model;
    MeshRenderer mat;
    [SerializeField] int price;
    // Start is called before the first frame update
    void Start()
    {
        model = gun.model.GetComponent<MeshFilter>();
        mat = gun.model.GetComponent<MeshRenderer>();
        gun.currAmmo = gun.maxAmmo;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.points >= price)
        {
            gameManager.instance.pScript.gunPickup(gun);
            gameManager.instance.points -= price;
            Destroy(gameObject);
        }
    }
}
