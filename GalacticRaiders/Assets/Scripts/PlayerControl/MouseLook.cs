using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform orientation;
    public Transform gunOrientation;

    public float sensX;
    public float sensY;

    float xRotation;
    float yRotation;
    float playerSens;

    // Start is called before the first frame update
    void Start()
    { 
        // remove cursor from screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        if (GameManager.sensitivity != 0) {
            playerSens = GameManager.sensitivity;
        } else {
            playerSens = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // get input from the mouse
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX * playerSens;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY * playerSens;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);

        if (gunOrientation != null)
        {
            gunOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        }
    }
}
