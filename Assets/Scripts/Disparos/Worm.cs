using UnityEngine;

public class Worm : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisiona tiene el layer WhatIsBullet
        if (collision.gameObject.layer == LayerMask.NameToLayer("WhatIsBullet"))
        {
            // Destruye el objeto que tiene este script (el objeto Worm)
            Destroy(gameObject);
        }
    }
}
