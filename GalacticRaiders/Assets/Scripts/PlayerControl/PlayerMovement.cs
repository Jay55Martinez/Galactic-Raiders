using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCoolDown;
    public float airMultiplier;
    public float jumpDelay;
    bool readyToJump;

    [Header("Sliding")]
    public float slideForce;
    public float slideYScale;
    public float slideJumpForce;
    private float startYscale;
    private bool sliding;
    private Vector3 slideDir;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode slideKey = KeyCode.LeftControl;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    Animator movementAnimate;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        movementAnimate = GetComponent<Animator>();

        if (movementAnimate != null)
        movementAnimate.SetInteger("transitionVar", 0);

        startYscale = transform.localScale.y;

        ResetJump();
    }

    // Update is called once per frame
    void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (movementAnimate != null)
            UpdateAnimation();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        Debug.Log(grounded);
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            // add jump animation
            if (movementAnimate != null)
            movementAnimate.SetInteger("transitionVar", 2);

            Invoke(nameof(Jump), jumpDelay);

            Invoke(nameof(ResetJump), jumpCoolDown);
        }

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0)) {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && sliding) {
            StopSlide();
        }

        if (Input.GetKeyDown(jumpKey) && grounded && sliding) {
            SlideJump();
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (sliding)
            SlidingMovement();
        else if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        else if (!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void StartSlide() {
        sliding = true;

        readyToJump = false;

        slideDir = moveDirection;
        transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
    }

    private void SlidingMovement() {
        rb.AddForce(slideDir.normalized * slideForce, ForceMode.Force);
    }

    private void StopSlide() {
        sliding = false;

        readyToJump = true;

        transform.localScale = new Vector3(transform.localScale.x, startYscale, transform.localScale.z);
    }

    private void SlideJump() {
        Vector3 slideJump = slideDir.normalized + 0.1f * orientation.up;
        rb.AddForce(slideJump.normalized * slideJumpForce, ForceMode.Impulse);
    }

    private void UpdateAnimation()
    {
        if (!readyToJump)
        {
            // jump animation shound happen
            return;
        }
        else if (moveDirection.normalized != Vector3.zero)
        {
            movementAnimate.SetInteger("transitionVar", 1);
        }
        else
        {
            // should be idle
            movementAnimate.SetInteger("transitionVar", 0);
        }
    }
}
