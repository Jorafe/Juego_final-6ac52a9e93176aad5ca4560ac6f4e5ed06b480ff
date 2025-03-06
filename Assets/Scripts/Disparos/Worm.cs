using UnityEngine;

public class Worm : MonoBehaviour
{
    // Cambiaremos este método para desactivar el gusano en lugar de destruirlo
    public void Deactivate()
    {
        gameObject.SetActive(false); // Desactiva el gusano en vez de destruirlo
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisiona tiene el layer WhatIsBullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsBullet"))
        {
            // Desactiva el gusano
            Deactivate(); 
        }
    }
}