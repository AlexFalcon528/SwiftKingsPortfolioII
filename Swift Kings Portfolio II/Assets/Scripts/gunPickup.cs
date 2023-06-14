using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class gunPickup : MonoBehaviour
{
    [SerializeField] gunStats gun;
    MeshFilter model;
    MeshRenderer mat;
    [SerializeField] int price;
    [SerializeField] TextMeshPro txt;
    // Start is called before the first frame update
    void Start()
    {
        model = gun.model.GetComponent<MeshFilter>();
        mat = gun.model.GetComponent<MeshRenderer>();
        gun.currAmmo = gun.maxAmmo;
        txt.text = $"{price} pts";
    }
    private void Update()
    {
        txt.transform.LookAt(gameManager.instance.player.transform.position);
        txt.transform.Rotate(Vector3.up, 180);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && gameManager.instance.points >= price)
        {
            gameManager.instance.pScript.gunPickup(gun);
            gameManager.instance.points -= price;
            gameManager.instance.currPoints.text = $"{gameManager.instance.points}";
            Destroy(gameObject);
        }
    }
}
