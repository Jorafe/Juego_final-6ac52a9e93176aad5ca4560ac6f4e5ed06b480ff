using UnityEngine;

public class CarRecovery : MonoBehaviour
{
    // Umbral de ángulo para considerar el coche volcado
    [Header("Recovery Settings")]
    public float recoverAngleThreshold = 45f; // Ángulo máximo para detectar volcado
    public float recoverSpeed = 5f; // Velocidad de corrección
    public float uprightForce = 200f; // Fuerza para levantar el coche
    public float recoveryDelay = 1f; // Retraso antes de intentar recuperar (en segundos)

    private Rigidbody rb;
    private Transform carTransform;
    private bool isRecovering = false;
    private float timeSinceFlipped = 0f; // Tiempo desde que el coche se volcó o quedó de lado

    void Start()
    {
        // Obtener Rigidbody y Transform
        rb = GetComponent<Rigidbody>();
        carTransform = transform;

        // Asegurarnos de que el coche empiece recto
        if (rb == null)
        {
            Debug.LogError("El coche necesita un Rigidbody para el correcto funcionamiento de la recuperación.");
        }
    }

    void Update()
    {
        // Verificar si el coche está volcado o de lado
        if (IsCarFlipped())
        {
            // Aumentar el tiempo desde que el coche se volcó
            timeSinceFlipped += Time.deltaTime;
        }
        else
        {
            // Si el coche ya no está volcado, reiniciamos el temporizador
            timeSinceFlipped = 0f;
        }

        // Si han pasado 1 segundo desde que el coche se volcó o quedó de lado, intentar recuperarlo
        if (timeSinceFlipped >= recoveryDelay && !isRecovering)
        {
            StartCoroutine(RecoverCarCoroutine());
        }
    }

    // Verifica si el coche está volcado o de lado
    private bool IsCarFlipped()
    {
        // Obtener los ángulos de rotación sobre los ejes X y Z (comunes para volcarse)
        float xAngle = Mathf.Abs(carTransform.rotation.eulerAngles.x);
        float zAngle = Mathf.Abs(carTransform.rotation.eulerAngles.z);

        // Si el ángulo de rotación sobre X o Z supera el umbral de volcado, consideramos que está volcado
        if (xAngle > recoverAngleThreshold || zAngle > recoverAngleThreshold)
        {
            return true;
        }

        // También verificamos si las ruedas están casi horizontales (de lado) utilizando la rotación sobre Y
        float yAngle = Mathf.Abs(carTransform.rotation.eulerAngles.y);

        // Si el coche está volcado hacia un lado (por ejemplo, con las ruedas horizontales)
        if (yAngle > 90f && yAngle < 270f)
        {
            return true;
        }

        return false;
    }

    // Coroutine que suaviza la recuperación
    private System.Collections.IEnumerator RecoverCarCoroutine()
    {
        isRecovering = true;

        // Recuperar la rotación del coche (solo sobre Y para mantener la orientación)
        Quaternion targetRotation = Quaternion.Euler(0f, carTransform.rotation.eulerAngles.y, 0f);

        // Suavizar la rotación para que no se mueva bruscamente
        while (IsCarFlipped())
        {
            carTransform.rotation = Quaternion.Slerp(carTransform.rotation, targetRotation, recoverSpeed * Time.deltaTime);

            // Si el coche está completamente volcado (con las ruedas hacia arriba), aplicar una fuerza hacia arriba
            if (Mathf.Abs(carTransform.rotation.eulerAngles.x) > 90f || Mathf.Abs(carTransform.rotation.eulerAngles.z) > 90f)
            {
                rb.AddForce(Vector3.up * uprightForce);
            }

            // Esperar un poco antes de intentar la corrección nuevamente
            yield return null; // Espera un solo frame para continuar la corrección gradualmente
        }

        // Al finalizar la corrección, detener el proceso
        isRecovering = false;
    }
}