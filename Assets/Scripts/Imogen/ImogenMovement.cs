using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public BoxCollider movementArea;   // El área delimitada por el BoxCollider
    public Transform target;           // El objetivo para mirar durante la fase de flotar
    public float flyingSpeed = 5f;     // Velocidad durante la fase de volar
    public float floatingSpeed = 2f;   // Velocidad durante la fase de flotar
    public float phaseChangeInterval = 3f; // Intervalo entre fases (en segundos)
    public float minDistanceBetweenPoints = 10f; // Distancia mínima entre puntos

    private Rigidbody rb;
    private Vector3 currentDestination;
    private float timer;
    private bool isFlying;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetNewDestination();
        timer = phaseChangeInterval;
        isFlying = true;

        // Desactivamos la gravedad para evitar que el enemigo caiga al suelo
        rb.useGravity = false;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            SwitchPhase();
            timer = phaseChangeInterval;
        }

        if (isFlying)
        {
            MoveTowardsDestination(flyingSpeed);
        }
        else
        {
            FloatAround();
        }
    }

    private void SwitchPhase()
    {
        isFlying = !isFlying; // Cambiar entre volar y flotar
        if (isFlying)
        {
            SetNewDestination(); // Establecer un nuevo punto de destino al cambiar a volar
        }
    }

    private void SetNewDestination()
    {
        Vector3 newDestination;

        // Generar un nuevo punto aleatorio que esté a una distancia mínima del anterior
        do
        {
            newDestination = new Vector3(
                Random.Range(movementArea.bounds.min.x, movementArea.bounds.max.x),
                Random.Range(movementArea.bounds.min.y, movementArea.bounds.max.y),
                Random.Range(movementArea.bounds.min.z, movementArea.bounds.max.z)
            );
        } while (Vector3.Distance(newDestination, currentDestination) < minDistanceBetweenPoints); // Asegurarse de que esté suficientemente lejos

        currentDestination = newDestination;
    }

    private void MoveTowardsDestination(float speed)
    {
        // Moverse hacia el punto de destino usando Rigidbody.velocity
        Vector3 direction = (currentDestination - transform.position).normalized;
        rb.velocity = direction * speed;

        // Hacer que el enemigo mire hacia el punto de destino mientras vuela
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);  // Mirar en dirección al destino
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * speed);
        }

        // Si el enemigo ha llegado al destino, establecer un nuevo destino
        if (Vector3.Distance(transform.position, currentDestination) < 0.1f)
        {
            SetNewDestination();
        }
    }

    private void FloatAround()
    {
        // Movimiento suave mientras flota usando Rigidbody.velocity para evitar el arrastre
        Vector3 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * floatingSpeed;

        // Mirar siempre hacia el objetivo asignado en el inspector (mirando al frente del enemigo)
        if (target != null && direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);  // Mirar en dirección al objetivo
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * floatingSpeed);
        }
    }
}
