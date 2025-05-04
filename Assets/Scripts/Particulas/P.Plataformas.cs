using UnityEngine;

public class P_plataformas : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto tiene un sistema de partículas
        ParticleSystem ps = other.GetComponent<ParticleSystem>();

        // Si hay un sistema de partículas y está desactivado, lo activa
        if (ps != null && !ps.isPlaying)
        {
            ps.Play();
        }
    }
}