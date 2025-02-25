using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;    // La orientación de la cámara
    public Transform player;         // El jugador
    public Transform playerObj;      // El objeto del jugador que rota
    public Rigidbody rb;             // Rigidbody del jugador para movimiento

    public float rotationSpeed;      // Velocidad de rotación de la cámara

    private void Start()
    {
        // Bloquear el cursor en el centro y hacerlo invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Rotar la orientación (la dirección en la que se ve la cámara)
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        // Rotar el objeto del jugador (movimiento en 3ra persona)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
    }
}