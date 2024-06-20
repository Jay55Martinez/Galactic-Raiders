using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float climbingSpeed;
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
    private bool onWall;
    private Vector3 slideDir;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public float skinWallStick = 0.001f;
    public bool grounded;
    public bool climbing;
    public Vector3 lastGroundedPosition;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode slideKey = KeyCode.LeftShift;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    [Header("Stair Handling")]
    public GameObject stepRayLower;
    public GameObject stepRayUpper;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    public Vector3 moveDirection;
    Rigidbody rb;
    RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
       
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startYscale = transform.localScale.y;

        ResetJump();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
            lastGroundedPosition = transform.position;
        }
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MovePlayer();

        MoveUpStairs();
    }

    // Method used for taking the keyboard input
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

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

    // Handles the players movement on different surfaces
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        onWall = Physics.Raycast(transform.position - new Vector3(0f, .75f, 0f),
            moveDirection.normalized, skinWallStick)
            || Physics.Raycast(transform.position + new Vector3(0f, .6f, 0f), moveDirection.normalized, skinWallStick);

        // Climbing
        if (climbing)
        {
            Vector3 movementDirectionWithoutXZ = new Vector3(0, moveDirection.normalized.y, 0);
            rb.AddForce(movementDirectionWithoutXZ, ForceMode.Force);
        }

        // Sliding
        else if (sliding && !onWall)
            SlidingMovement();

        // On Slope
        else if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }

        }

        // On the ground
        else if (grounded && !onWall)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // On the ground and against a wall
        else if (grounded && onWall)
            MoveOnWall(moveDirection, 1);

        // In the air
        else if (!grounded && !onWall)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        // In the air and against a wall
        else if (!grounded && onWall)
            MoveOnWall(moveDirection, airMultiplier);

        rb.useGravity = !OnSlope();
    }

    // For debugging
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(transform.position - new Vector3(0f, .5f, 0f), moveDirection.normalized);
    }

    private void MoveUpStairs()
    {
        RaycastHit hitLower;

        Vector3 movementDirectionWithoutY = new Vector3(moveDirection.normalized.x, 0, moveDirection.normalized.z);

        if (Physics.Raycast(stepRayLower.transform.position, movementDirectionWithoutY, out hitLower, skinWallStick))
        {
            RaycastHit hitUpper;
            if (!Physics.Raycast(stepRayUpper.transform.position, movementDirectionWithoutY, out hitUpper, skinWallStick + 0.2f))
            {
                rb.position += new Vector3(0, 0.1f, 0);
            }
        }

        RaycastHit hitLower45;

        if (Physics.Raycast(stepRayLower.transform.position, RotateVectorY(movementDirectionWithoutY, 45), out hitLower45, skinWallStick))
        {
            RaycastHit hitUpper45;
            if (!Physics.Raycast(stepRayUpper.transform.position, RotateVectorY(movementDirectionWithoutY, 45), out hitUpper45, skinWallStick + 0.2f))
            {
                rb.position += new Vector3(0, 0.1f, 0);
            }
        }

        RaycastHit hitLowerMinus45;

        if (Physics.Raycast(stepRayLower.transform.position, RotateVectorY(movementDirectionWithoutY, -45), out hitLowerMinus45, skinWallStick))
        {
            RaycastHit hitUpperMinus45;
            if (!Physics.Raycast(stepRayUpper.transform.position, RotateVectorY(movementDirectionWithoutY, -45), out hitUpperMinus45, skinWallStick + 0.2f))
            {
                rb.position += new Vector3(0, 0.1f, 0);
            }
        }
    }

    Vector3 RotateVectorY(Vector3 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);

        float newX = vector.x * cos + vector.z * sin;
        float newZ = -vector.x * sin + vector.z * cos;

        return new Vector3(newX, vector.y, newZ);
    }

    private void MoveOnWall(Vector3 movementForce, float airMovementMultiplier)
    {
        if (Mathf.Abs(movementForce.x) > Mathf.Abs(movementForce.z))
        {
            movementForce.z *= .3f;
            movementForce.x *= .1f;
        }
        else
        {
            movementForce.x *= .3f;
            movementForce.z *= .1f;
        }
        rb.AddForce(movementForce.normalized * moveSpeed * 10f * airMovementMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        // limiting speed on the slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }

        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
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

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.8f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;

        }
         return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
