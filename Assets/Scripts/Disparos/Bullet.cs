using UnityEngine;
using TMPro; // Asegúrate de usar TextMeshPro
using System.Collections; // Necesario para IEnumerator

public class Bullet : MonoBehaviour
{
    public float speed = 30f;
    public float lifeTime = 5f; // Tiempo de vida configurable
    private Rigidbody rb;
    private bool hitWorm = false; // Variable para saber si la bala tocó un gusano

    // Contador de gusanos desactivados
    public static int wormCount = 0; 
    public TMP_Text wormCountText;  // Referencia al texto del contador de gusanos en el canvas

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

        // Si no ha tocado un gusano, desactivamos la bala después de un tiempo
        Invoke("Deactivate", lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Comprobar si la bala tocó un gusano
        if (other.CompareTag("whatisworm"))
        {
            hitWorm = true; // La bala tocó un gusano
            StartCoroutine(DeactivateAfterCollision(other));   // Esperar un frame antes de desactivar la bala
        }
    }

    private IEnumerator DeactivateAfterCollision(Collider wormCollider)
    {
        // Esperamos un frame para asegurarnos de que la colisión se haya registrado
        yield return null;

        // Desactivamos la bala
        Deactivate();

        // Desactivamos el gusano
        Worm worm = wormCollider.GetComponent<Worm>();
        if (worm != null)
        {
            worm.Deactivate(); // Desactivamos el gusano

            // Incrementamos el contador de gusanos desactivados
            wormCount++; // Incrementamos el contador
            UpdateWormCountText(); // Actualizamos el texto
        }
    }

    private void Deactivate()
    {
        gameObject.SetActive(false); // Desactivamos la bala
    }

    private void UpdateWormCountText()
    {
        // Actualizamos el texto del contador de gusanos desactivados
        wormCountText.text = wormCount + "/20"; // El número máximo de gusanos es 20
    }
}