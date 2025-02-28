using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
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
        
        // Asegúrate de disparar hacia el eje X, independientemente de la rotación
        rb.velocity = new Vector3(speed, 0, 0);  // Movimiento en el eje X

        lifeTime = customLifeTime;
        Invoke("Deactivate", lifeTime); // Desactiva la bala después del tiempo de vida
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisiona tiene el layer WhatIsWorm
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsWorm"))
        {
            // Destruye el objeto con el layer WhatIsWorm
            Destroy(collision.gameObject);
        }

        // Desactiva la bala
        Deactivate();
    }

    private void Deactivate()
    {
        gameObject.SetActive(false);
    }
}