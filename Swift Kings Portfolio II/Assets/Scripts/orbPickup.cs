using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class orbPickup : MonoBehaviour
{
    bool playerInRange;
    [SerializeField] int price;
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetButtonDown("BuyGun") && gameManager.instance.points >= price && playerInRange)
        {
            gameManager.instance.pScript.orbCount++;
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
