using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hpPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.pScript.powerUpHP();
    }

    private void OnTriggerExit(Collider other)
    {
        Destroy(gameObject);
    }
}
