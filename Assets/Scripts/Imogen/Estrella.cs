using UnityEngine;

public class Estrella : MonoBehaviour
{
    public string targetTag = "Player";  // El tag del objeto que se destruirá (en este caso "Player").
    public string targetLayer = "Player";  // El layer del objeto que se destruirá (en este caso "Player").

    // Se ejecuta cuando el objeto con este script colisiona con otro objeto
    private void OnCollisionEnter(Collision collision)
    {
        // Verificamos si el objeto con el que colisionamos tiene el tag "Player" o el layer "Player"
        if (collision.gameObject.CompareTag(targetTag) || collision.gameObject.layer == LayerMask.NameToLayer(targetLayer))
        {
            // Destruimos el objeto objetivo
            Destroy(collision.gameObject);
            Debug.Log("El objeto Player ha sido destruido.");
        }
    }
}