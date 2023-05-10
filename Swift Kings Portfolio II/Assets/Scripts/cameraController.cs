using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    float mouseX;
    float mouseY;
    [SerializeField] bool invert;
    [SerializeField] int sensVer;
    [SerializeField] int sensHor;
    [SerializeField] int maxPitch;
    [SerializeField] int minPitch;
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
        mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;//Read Cursor inputs
        mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVer;

        if (invert)//If airplane controls
        {
            xRotation += mouseY; //Invert
        }
        else
        {
            xRotation -= mouseY;
        }
        xRotation = Mathf.Clamp(xRotation, minPitch, maxPitch); //Contain camera pitch within min & max

        //Rotate the camera on the x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //Rotate the player on the y axis
        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
