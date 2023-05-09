using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    [SerializeField] int sensHor;
    [SerializeField] int sensVert;

    [SerializeField] int lockVerMin;
    [SerializeField] int lockVerMax;

    [SerializeField] bool invertY;

    float xRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        //Read cursor inputs
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        //Convert input data to rotation float, add option for inverted controls
        if (invertY)
        {
            xRotation += mouseY;
        }
        else { 
            xRotation -= mouseY;
        }
        

        //Limit camera rotation
        xRotation = Mathf.Clamp(xRotation, lockVerMin, lockVerMax);

        //Rotate the camera on the x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //Rotate the player on the y axis
        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
