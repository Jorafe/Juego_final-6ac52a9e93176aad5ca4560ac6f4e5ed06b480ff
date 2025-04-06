using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pelota1 : MonoBehaviour
{
    public float fuerzaRebote = 10f;
    public Animator animator; // Referencia al Animator

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

        if (rb != null)
        { 
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
                
            }
            // Aplica una fuerza hacia arriba
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z); // Reiniciar velocidad vertical
            rb.AddForce(Vector3.up * fuerzaRebote, ForceMode.Impulse);

            // Si el objeto tiene un Animator, activa la animación de salto
           
        }
    }
}