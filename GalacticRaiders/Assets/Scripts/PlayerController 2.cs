using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 input;
    Vector3 moveDirection;
    
    public float moveSpeed = 10;
    public float jumpHeight = 10;
    public float gravity = 9.81f;
    public float airControl = 1;


    // Start is called before the first frame update
    void Start()
    {
        // grab necessary info
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // get keyboard inputs
        // using GetAxisRaw gives snappier movements
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // create unit vector from keyboard inputs
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;

        input *= moveSpeed;

        // jump function
        if (controller.isGrounded) { // we can jump
            moveDirection = input;
            if (Input.GetButton("Jump")) {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            } else {
                moveDirection.y = 0.0f;
            }
        } else { // we are midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);
    }
}

