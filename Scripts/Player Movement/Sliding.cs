using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private Movement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    //When game loads, stores the location of the player rigidbody in the variable rb (RigidBody)
    //When game loads, stores the location of the Movement script in the variable pm (Player Movement)
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<Movement>();

        //stores the height of the player for later
        startYScale = playerObj.localScale.y;
    }

    //Runs each frame
    private void Update()
    {
        //Gets the WASD inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //if the slide key is held down and any of the movement keys are being pressed, start sliding
        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            StartSlide();

        //if the slide key is not held and the current player state is sliding, stop sliding
        if(Input.GetKeyUp(slideKey) && pm.sliding)
            StopSlide();
    }

    //runs every physics frame
    private void FixedUpdate()
    {
        //if the player state is sliding, then use the SlidingMovement sub routine
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        //set the sliding state in the movement script to true
        pm.sliding = true;

        //changes the locally stored y transform value to the slide scale value
        playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        //pushes the player to the floor after the y value is changed due to the bottom being lifted after changes are applied
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        //sets the slide timer to the maximum time allowed to slide, more of a countdown instead of a stopwatch
        slideTimer = maxSlideTime;
    }

    //Section in charge of the movement when sliding
    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //sliding normally on ground
        if(!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);

            slideTimer -= Time.deltaTime;
        }

        //sliding down slopes
        else
        {
            rb.AddForce(pm.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0)
            StopSlide();
    }

    private void StopSlide()
    {
        pm.sliding = false;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}