using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{

    [SerializeField] Renderer model;
    [SerializeField] Collider col;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            model.enabled = false;
            col.enabled = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            model.enabled = true;
            col.enabled = true;
        }
    }
}
