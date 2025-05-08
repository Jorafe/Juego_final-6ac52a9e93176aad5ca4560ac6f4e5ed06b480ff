using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCam : MonoBehaviour
{
    private Animator animator;

    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    [Header("Cam Control")]
    public float rotationSpeed = 10f;
    public Transform camHolder;  // Empty GameObject que contiene la cámara

    public float sensitivityX = 2f;
    public float sensitivityY = 2f;
    public float minY = -40f;
    public float maxY = 60f;

    private float xRotation;
    private float yRotation;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        animator = playerObj.GetComponent<Animator>();

        Vector3 initialRotation = camHolder.eulerAngles;
        xRotation = initialRotation.x;
        yRotation = initialRotation.y;
    }

   private void Update()
{
    // Obtener la entrada del joystick izquierdo para mover la cámara
    float horizontalInput = Input.GetAxis("Horizontal");  // Esto corresponde al movimiento en X del joystick izquierdo
    float verticalInput = Input.GetAxis("Vertical");      // Esto corresponde al movimiento en Y del joystick izquierdo

    // Rotar la orientación (la dirección en la que se ve la cámara)
    Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
    orientation.forward = viewDir.normalized;

    // Crear la dirección de movimiento basada en la cámara
    Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

    // Mover el jugador en la dirección de la cámara
    if (inputDir != Vector3.zero)
    {
        // Esto hace que el jugador gire hacia la dirección que está moviendo
        playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }

    // Animar el jugador (si tienes un animator configurado)
    if (animator != null)
    {
        animator.SetFloat("XSpeed", horizontalInput);
        animator.SetFloat("YSpeed", verticalInput);
    }
}
}