using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootDmgPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.pScript.powerUpDMG();
    }

    private void Update()
    {
        if (gameManager.instance.pScript.isPoweredUp)
        {
            Destroy(gameObject);
        }
    }
}
