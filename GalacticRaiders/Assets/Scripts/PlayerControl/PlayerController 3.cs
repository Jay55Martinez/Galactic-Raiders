using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController controller;
    Vector3 input;
    Vector3 moveDirection;
    
    public float moveSpeed = 10;
    public float slideSpeed = 5;
    public float boostSpeed = 1.5f;
    public float jumpHeight = 10;
    public float gravity = 9.81f;
    public float airControl = 1;

    private bool isSliding = false;
    private Vector3 slideDir;

    private float height; // stores normal height

    // Start is called before the first frame update
    void Start()
    {
        // grab necessary info
        controller = GetComponent<CharacterController>();
        height = controller.height;
    }

    // Update is called once per frame
    void Update()
    {
        float h = height; // store temporary height
        float speed = controller.velocity.magnitude; // will be used to interpolate between values for smoother motion

        // get keyboard inputs
        // using GetAxisRaw gives snappier movements // update: fixed gravity on axes, less slippery
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // create unit vector from keyboard inputs
        input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
        // input *= moveSpeed;

        // jump function
        if (controller.isGrounded) { // we can jump
            if (input.magnitude > 0f) {
                if (speed < moveSpeed) {// snap to proper speed
                    speed = moveSpeed;
                } else {
                    speed = Mathf.Lerp(speed, moveSpeed, 5 * Time.deltaTime);
                }
            }
            moveDirection = input * speed;
            if (Input.GetButton("Jump")) {
                moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
            } else {
                moveDirection.y = 0.0f;
            }
        } else { // we are midair
            input.y = moveDirection.y;
            moveDirection = Vector3.Lerp(moveDirection, input, airControl * Time.deltaTime);
        }

        // sliding function
        if (controller.isGrounded && Input.GetKey(KeyCode.LeftControl)) {
            isSliding = true;
            slideDir = controller.velocity.normalized; // store the direction the character is moving in
        }

        if (isSliding) {
            speed = Mathf.Lerp(speed, slideSpeed, 5 * Time.deltaTime); // slowly return to sliding speed
            h *= 0.5f;
            moveDirection = slideDir * speed;
            if (!Input.GetKey(KeyCode.LeftControl)) { // the player lets go of the slide key
                isSliding = false;
            } else if (Input.GetKeyDown(KeyCode.Space)) { // players can jump from a slide, with an added boost
                moveDirection *= boostSpeed;
                moveDirection.y += Mathf.Sqrt(2 * jumpHeight * gravity / 6);
                isSliding = false;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * Time.deltaTime);

        Debug.Log(moveDirection.magnitude);

        // fixing height depending on movement change
        float lastHeight = controller.height;
        // controller.height = h;
        controller.height = Mathf.Lerp(controller.height, h, 25 * Time.deltaTime);
        transform.position += new Vector3(0, (controller.height - lastHeight)/2, 0);
    }
}

