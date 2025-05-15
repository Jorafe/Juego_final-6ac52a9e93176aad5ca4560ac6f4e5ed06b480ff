using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX = 200f;
    public float sensY = 200f;
    public Transform orientation;
    public Transform camHolder;
    public Transform player;

    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Entradas del ratón
        float mouseX = Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;

        // Entradas del joystick derecho (Input Manager clásico)
        float joyX = Input.GetAxis("RightStickX") * sensX * Time.deltaTime;
        float joyY = Input.GetAxis("RightStickY") * sensY * Time.deltaTime;

        // Combinar ambas entradas
        float finalX = mouseX + joyX;
        float finalY = mouseY + joyY;

        // Invertir eje Y si lo necesitas
        xRotation -= finalY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        yRotation += finalX;

        transform.position = player.position;
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}