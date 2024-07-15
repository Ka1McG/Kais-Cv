using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEditor.U2D;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float sprintSpeed;
    public float walkSpeed;
    public float slideSpeed;
    public float dashSpeed;
    public float wallRunSpeed;

    public float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    private bool keepMomentum;

    public float speedIncreaseMultiplier;
    public float slopeIncreaseMultiplier;
    public float dashSpeedChangeFactor;

    public float maxYSpeed;
    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;

    //set of values for the movement states
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning, 
        crouching,
        dashing,
        sliding,
        air
    }

    //set of boolean values for different unique states
    public bool sliding;
    public bool wallrunning;
    public bool dashing;

    //on game load, grabs reference to the player rigidbody, freezes the player rotation to stop them from falling over,
    // sets the readyToJump variable to true for later and stores the startYScale as the height of the player rigidbody
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    //every frame we check to see if the player is on the floor by essentially shooting a laser down from the center of mass and seeing if it hits the floor within a certain distance
    //we call the MyInput proceedure to enable player control, we call the SpeedHandler proceedure to limit the x,y and z velocity to make the movement smoother
    //we call the StateHandler proceedure to deal with the previous boolean variables and change movement speeds accordingly
    private void Update()
    {
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        //handles drag by seeing if the player is in a common movement state (walking/ sprinting/ crouching) and changing the rigidbodies drag to the assigned variable
        //otherwise, there is no drag
        if (state == MovementState.walking || state == MovementState.sprinting || state == MovementState.crouching)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    //every physics frame we call the MovePlayer proceedure to apply any calculated forces
    private void FixedUpdate()
    {
        MovePlayer();
    }

    //proceedure in control of the player inputs
    private void MyInput()
    {
        //A and D keys stored as -1 and 1 respectively
        horizontalInput = Input.GetAxisRaw("Horizontal");
        //W and S keys stored as 1 and -1 respectively
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        //if the player is pressing the designated jump key and are readyToJump and are on the ground, then:
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            //set the ready to jump variable to false to prevent more than one jump in the air
            readyToJump = false;

            //call the Jump proceedure
            Jump();

            //reset jump and apply a cooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        
        //start crouching if the chosen key is pressed
            if (Input.GetKeyDown(crouchKey))
            {
                //shrink the player to the chosen YScale
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                //push the player rigidbody to the floor as it shrinks
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }

            //stop crouching when crouch key isn't pressed
            if (Input.GetKeyUp(crouchKey))
            {
                //reset the players size to default
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
    }

    private void StateHandler()
    {
        //Mode - Dashing
        if (dashing)
        {
            //changes the movement state to dashing
            state = MovementState.dashing;
            //changes the desiredMoveSpeed to the set dashSpeed
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;
        }

        //Mode - Wallrunning
        else if(wallrunning)
        {
            //changes the movement state to wallrunning
            state = MovementState.wallrunning;
            //changes the desiredMoveSpeed to the set wallRunSpeed
            desiredMoveSpeed = wallRunSpeed;
        }

        //Mode - Sliding
        else if (sliding)
        {
            //changes the movement state to sliding
            state = MovementState.sliding;

            if(OnSlope() && rb.velocity.y > 0.1f)
                desiredMoveSpeed = slideSpeed;

            else
                desiredMoveSpeed = sprintSpeed;
        }

        //Mode - crouching
        else if (Input.GetKey(crouchKey))
        {
            //changes the movement state to crouching
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
        }
        
        //Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            //changes the movement state to sprinting
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }

        //Mode - Walking
        else if (grounded)
        {
            //changes the movement state to walking
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }

        //Mode - Air
        else
        {
            //changes the movement state to air
            state = MovementState.air;

            if (desiredMoveSpeed < sprintSpeed)
                desiredMoveSpeed = walkSpeed;
            else
                desiredMoveSpeed = sprintSpeed;
        }

        //variable set in StateHandler called desiredMoveSpeedHasChanged will return true if desiredMoveSpeed is not equal to lastDesiredMoveSpeed
        bool desiredMoveSpeedHasChanged = desiredMoveSpeed != lastDesiredMoveSpeed;

        if (lastState == MovementState.dashing) keepMomentum = true;

        if (desiredMoveSpeedHasChanged)
        {
            if (keepMomentum)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeed());
            }
            else
            {
                StopAllCoroutines();
                moveSpeed = desiredMoveSpeed;
            }
        }
        
        //maths to see if desired movespeed has changed drastically
        if(Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > 0f && moveSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            moveSpeed = desiredMoveSpeed;
        }

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;
    }

    private float speedChangeFactor;
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);

            if (OnSlope())
            {
                float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);

                time += Time.deltaTime * speedIncreaseMultiplier * slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
                time += Time.deltaTime * speedIncreaseMultiplier;

            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }
    private void MovePlayer()
    {
        if (state == MovementState.dashing) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        
        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        //turn gravity off while on a slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }

        //limit y velocity
        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
        {
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        //Reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

            return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}