using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grenadePickUP : MonoBehaviour
{

    [SerializeField] GravityGrenade grenade;
    MeshFilter model;
    MeshRenderer mat;
    [SerializeField] int price;
    // Start is called before the first frame update
    void Start()
    {
        model = grenade.model.GetComponent<MeshFilter>();
        mat = grenade.model.GetComponent<MeshRenderer>();
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.points >= price)
        {
            gameManager.instance.pScript.GrenadePickUp();
            gameManager.instance.points -= price;
            gameManager.instance.currPoints.text = $"{gameManager.instance.points}";
            Destroy(gameObject);
        }
    }
}
