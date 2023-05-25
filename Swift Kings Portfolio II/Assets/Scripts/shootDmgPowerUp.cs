using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootDmgPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.pScript.powerUpDMG();
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
