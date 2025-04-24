using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 5f;
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
        lifeTime = customLifeTime;
        Invoke("Deactivate", lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Worm"))  // Asegúrate de que los enemigos tengan este tag
        {
            other.gameObject.SetActive(false);  // Desactivamos el gusano
            LevelManager.Instance.RegisterEnemyDeactivated(); // Llamamos al nuevo método sin parámetros
            Deactivate();  // Desactivamos la bala
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}