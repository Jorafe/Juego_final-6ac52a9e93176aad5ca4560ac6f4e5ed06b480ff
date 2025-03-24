using UnityEngine;

public class FlyingEnemy : MonoBehaviour
{
    public BoxCollider movementArea;   // El área delimitada por el BoxCollider
    public Transform target;           // El objetivo para mirar durante la fase de flotar
    public float flyingSpeed = 5f;     // Velocidad durante la fase de volar
    public float floatingSpeed = 1f;   // Velocidad durante la fase de flotar (reducción en la fase de flotación)
    public float floatingPhaseDuration = 3f; // Duración de la fase de flotar (en segundos)
    public float minDistanceBetweenPoints = 10f; // Distancia mínima entre puntos

    private Rigidbody rb;
    private Vector3 currentDestination;
    private float floatingTimer;  // Temporizador para la fase de flotar
    private bool isFlying;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetNewDestination();
        floatingTimer = floatingPhaseDuration; // Comienza con el temporizador de flotación
        isFlying = true;

        // Desactivamos la gravedad para evitar que el enemigo caiga al suelo
        rb.useGravity = false;
    }

    private void Update()
    {
        if (isFlying)
        {
            // Moverse hacia el destino de vuelo
            MoveTowardsDestination(flyingSpeed);
        }
        else
        {
            // Si está en la fase de flotar, decrementamos el temporizador de la fase de flotar
            floatingTimer -= Time.deltaTime;

            // Si el temporizador de flotar llega a cero, cambia a la fase de vuelo
            if (floatingTimer <= 0f)
            {
                isFlying = true;
                SetNewDestination(); // Establecer un nuevo destino al comenzar la fase de vuelo
                floatingTimer = floatingPhaseDuration; // Reiniciar el temporizador de flotación
            }

            // Moverse aleatoriamente en la fase de flotar
            MoveRandomly();
            LookAtTarget(); // Mirar siempre hacia el objetivo
        }
    }

    private void SetNewDestination()
    {
        Vector3 newDestination;

        // Generar un nuevo punto aleatorio que esté a una distancia mínima del anterior
        do
        {
            newDestination = new Vector3(
                Random.Range(movementArea.bounds.min.x + 0.5f, movementArea.bounds.max.x - 0.5f),  // Reducir el rango en 0.5f
                Random.Range(movementArea.bounds.min.y + 0.5f, movementArea.bounds.max.y - 0.5f),  // Reducir el rango en 0.5f
                Random.Range(movementArea.bounds.min.z + 0.5f, movementArea.bounds.max.z - 0.5f)   // Reducir el rango en 0.5f
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

        // Si el enemigo ha llegado al destino, pasar a la fase de flotar
        if (Vector3.Distance(transform.position, currentDestination) < 0.1f)
        {
            isFlying = false;
            floatingTimer = floatingPhaseDuration; // Reiniciar el temporizador de flotación
        }
    }

    private void MoveRandomly()
    {
        // Moverse aleatoriamente con una velocidad reducida en la fase de flotar
        Vector3 direction = (currentDestination - transform.position).normalized;
        rb.velocity = direction * floatingSpeed;

        // Si el enemigo ha llegado al destino, establecer un nuevo destino
        if (Vector3.Distance(transform.position, currentDestination) < 0.1f)
        {
            SetNewDestination(); // Establecer un nuevo destino aleatorio cuando llega
        }
    }

    private void LookAtTarget()
{
    // Hacer que el enemigo mire hacia el objetivo, pero solo rotando en el eje Y
    if (target != null)
    {
        // Calcular la dirección hacia el objetivo
        Vector3 direction = (target.position - transform.position).normalized;

        // Ajustar la dirección para que no se modifique la rotación en Y
        direction.y = 0; // Ignorar la componente Y de la dirección

        if (direction != Vector3.zero)
        {
            // Calcular la rotación en Y hacia el objetivo
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Hacer que el enemigo rote solo en el eje Y
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * floatingSpeed);
        }
    }
}
}