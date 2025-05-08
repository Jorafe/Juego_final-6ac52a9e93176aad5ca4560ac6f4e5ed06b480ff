using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Animator animator;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float dashSpeed;
    public float dashSpeedChangeFactor;
    public float maxYSpeed;
    public float groundDrag;
    public float wallrunSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;

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

    public Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        freeze,
        unlimited,
        walking,
        sprinting,
        wallrunning,
        dashing,
        air
    }

    public bool dashing;
    public bool wallrunning;
    public bool freeze;
    public bool unlimited;

    public bool restricted;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        animator = GetComponent<Animator>();

        readyToJump = true;
        //startYScale = transform.localScale.y;
    }

    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, playerHeight * 0.5f + 0.2f, whatIsGround))
        {
            grounded = true;

            animator.SetBool("isDashing", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpingMove", false);

            if (hit.collider.CompareTag("whatIsIce"))
            {
                rb.drag = 0;
            }
            else
            {
                rb.drag = groundDrag;
            }
        }
        else
        {
            grounded = false;
            rb.drag = 0;
        }

        MyInput();
        SpeedControl();
        StateHandler();

        if (grounded)
        {
            animator.SetFloat("XSpeed", horizontalInput);
            animator.SetFloat("YSpeed", verticalInput);
        }
        else
        {
            animator.SetFloat("XSpeed", 0);
            animator.SetFloat("YSpeed", 0);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        if ((Input.GetKey(jumpKey) || Input.GetButton("Jump")) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;
    private MovementState lastState;
    bool keepMomentum;
    private float speedChangeFactor;

    private void StateHandler()
    {
        if (freeze)
        {
            state = MovementState.freeze;
            rb.velocity = Vector3.zero;
            desiredMoveSpeed = 0f;
        }
        else if (unlimited)
        {
            state = MovementState.unlimited;
            desiredMoveSpeed = 999f;
            return;
        }
        else if (wallrunning)
        {
            state = MovementState.wallrunning;
            desiredMoveSpeed = wallrunSpeed;
        }
        else if (dashing)
        {
            state = MovementState.dashing;
            desiredMoveSpeed = dashSpeed;
            speedChangeFactor = dashSpeedChangeFactor;

            animator.SetBool("isJumping", false);
            animator.SetBool("isDashing", true);
        }
        else if (grounded && (Input.GetKey(sprintKey) || Input.GetButton("Sprint")))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
        }
        else if (grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
        }
        else
        {
            state = MovementState.air;
            desiredMoveSpeed = desiredMoveSpeed < sprintSpeed ? walkSpeed : sprintSpeed;
        }

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

        lastDesiredMoveSpeed = desiredMoveSpeed;
        lastState = state;

        if (Mathf.Abs(desiredMoveSpeed - moveSpeed) < 0.01f) keepMomentum = false;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0;
        float difference = Mathf.Abs(desiredMoveSpeed - moveSpeed);
        float startValue = moveSpeed;
        float boostFactor = speedChangeFactor;

        while (time < difference)
        {
            moveSpeed = Mathf.Lerp(startValue, desiredMoveSpeed, time / difference);
            time += Time.deltaTime * boostFactor;
            yield return null;
        }

        moveSpeed = desiredMoveSpeed;
        speedChangeFactor = 1f;
        keepMomentum = false;
    }

    private void MovePlayer()
    {
        if (restricted) return;
        if (state == MovementState.dashing) return;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);
            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            animator.SetBool("isJumping", false);
            animator.SetBool("isJumpingMove", false);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        if (!wallrunning) rb.useGravity = !OnSlope();
    }

    private bool IsJumpingStatic()
    {
        return horizontalInput == 0 && verticalInput == 0;
    }

    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
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

        if (maxYSpeed != 0 && rb.velocity.y > maxYSpeed)
            rb.velocity = new Vector3(rb.velocity.x, maxYSpeed, rb.velocity.z);
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        if (IsJumpingStatic())
            animator.SetBool("isJumping", true);
        else
            animator.SetBool("isJumpingMove", true);
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