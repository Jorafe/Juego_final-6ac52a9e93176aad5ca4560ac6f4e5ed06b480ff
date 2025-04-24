using UnityEngine;

public class EstrellaCollision : MonoBehaviour
{
    [HideInInspector] public EstrellaPresion presionManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            presionManager.MarcarComoTocadaPorJugador();
        }
    }
}
