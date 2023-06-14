using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    MeshFilter model;
    MeshRenderer mat;
    [SerializeField] int price;
    private bool playerInRange;
    // Start is called before the first frame update
    void Start()
    {
        model = gun.model.GetComponent<MeshFilter>();
        mat = gun.model.GetComponent<MeshRenderer>();
        gun.currAmmo = gun.maxAmmo;
    }
    private void Update()
    {
        if (Input.GetButtonDown("BuyGun") && gameManager.instance.points >= price && playerInRange)
        {
            gameManager.instance.pScript.gunPickup(gun);
            gameManager.instance.points -= price;
            gameManager.instance.currPoints.text = $"{gameManager.instance.points}";
            gameManager.instance.gunPickupIcon.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            gameManager.instance.gunPrice.text = '(' + price.ToString() + ") pts";
            gameManager.instance.gunPickupIcon.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        playerInRange = false;
        gameManager.instance.gunPickupIcon.SetActive(false);
    }
}
