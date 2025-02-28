using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public float lifeTime = 5f; // Ahora es parametrizable
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Activate(Vector3 position, Quaternion rotation, float customLifeTime)
    {
        transform.position = position;
        transform.rotation = rotation;
        gameObject.SetActive(true);
        rb.velocity = transform.right * speed; // Disparo en el eje X
        lifeTime = customLifeTime;
        Invoke("Deactivate", lifeTime); // Desactiva la bala después del tiempo establecido
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si el objeto con el que colisiona tiene el tag "whatisworm"
        if (other.CompareTag("whatisworm"))
        {
            Deactivate(); // Desactiva la bala
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}