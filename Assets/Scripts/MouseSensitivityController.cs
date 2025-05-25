using UnityEngine;

public class MouseSensitivity : MonoBehaviour
{
    [Tooltip("Multiplicador para la sensibilidad del ratón. 1 = normal, <1 menos sensible, >1 más sensible")]
    [Range(0f, 5f)]
    public float sensitivity = 1f;

    // Devuelve el valor de movimiento del ratón en X ajustado
    public float GetMouseX()
    {
        return Input.GetAxis("Mouse X") * sensitivity;
    }

    // Devuelve el valor de movimiento del ratón en Y ajustado
    public float GetMouseY()
    {
        return Input.GetAxis("Mouse Y") * sensitivity;
    }
}