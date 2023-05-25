using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerUpSpeed : MonoBehaviour
{
    


    private void OnTriggerEnter(Collider other)
    {
        gameManager.instance.pScript.powerUpSpeed();
    }

    

    private void Update()
    {
        if(gameManager.instance.pScript.isPoweredUp)
        {
            Destroy(gameObject);
        }
    }

}
