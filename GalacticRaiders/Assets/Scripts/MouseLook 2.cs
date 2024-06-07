using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    Transform playerBody;
    public float mouseSensitivity = 10;

    float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        // grab necessary info
        playerBody = transform.parent.transform;

        // remove cursor from screen
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // get input from the mouse
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // yaw
        playerBody.Rotate(Vector3.up * moveX);

        // pitch
        pitch -= moveY;

        // restrict movement to -90 - 90
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
    }
}
