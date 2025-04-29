using UnityEngine;

public class Cristal : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NovaLive novaLive = FindObjectOfType<NovaLive>(); // Asumiendo que hay uno solo en escena
            if (novaLive != null)
            {
                novaLive.QuitarVida();
            }
            else
            {
                Debug.LogWarning("No se encontró el script NovaLive en la escena.");
            }
        }
    }
}
