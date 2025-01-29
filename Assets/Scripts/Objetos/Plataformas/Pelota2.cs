using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota2 : MonoBehaviour
{
    public float fuerzaRebote = 10f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Aplica una fuerza hacia arriba
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reiniciar velocidad vertical
            rb.AddForce(Vector3.up * fuerzaRebote, ForceMode.Impulse);
        }
    }
}
