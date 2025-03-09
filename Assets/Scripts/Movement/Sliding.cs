using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{

    private Animator animator;


    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    public PlayerMovement pm;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode controlKey = KeyCode.LeftControl;
    public KeyCode shiftKey = KeyCode.LeftShift; // Cambié a LeftShift para que sea Shift
    private float horizontalInput;
    private float verticalInput;

    [Header("Cooldown")]
    public float slideCooldownTime = 3f;  // Tiempo de cooldown (en segundos)
    private float slideCooldownTimer = 0f; // Temporizador de cooldown

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();

        animator = GetComponent<Animator>();

        startYScale = playerObj.localScale.y;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Verificar si el cooldown ha terminado
        if (slideCooldownTimer > 0)
        {
            slideCooldownTimer -= Time.deltaTime;  // Descontamos tiempo del cooldown
        }

        // Solo iniciar el slide si no estamos en cooldown y ambas teclas están presionadas
        if (slideCooldownTimer <= 0 && Input.GetKey(controlKey) && Input.GetKey(shiftKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            if (!pm.sliding) // Asegurarse de que no esté ya deslizándose
                StartSlide();
        }

        // Si se suelta la tecla control, se detiene el slide
        if (Input.GetKeyUp(controlKey) && pm.sliding)
            StopSlide();
    }

    private void FixedUpdate()
    {
        if (pm.sliding)
            SlidingMovement();
    }

    private void StartSlide()
    {
        pm.sliding = true;
        

        //playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
        rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);

        slideTimer = maxSlideTime;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // sliding normal
        if (!pm.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }

        // sliding down a slope
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
        
        
        // Iniciar el cooldown después de terminar el slide
        slideCooldownTimer = slideCooldownTime;

        playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
    }
}