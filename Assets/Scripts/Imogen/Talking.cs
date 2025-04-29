using UnityEngine;
using System.Collections;

public class Talking : MonoBehaviour
{
    [Header("Animación")]
    public Animator animator; // Referencia al Animator
    public string triggerTalking = "IsTalking"; // Trigger para hablar
    public string triggerDamage = "TakeDamage"; // Trigger para daño

    [Header("Parámetros de tiempo")]
    public float intervalo = 30f; // Tiempo en segundos entre activaciones

    private bool isTalking = false;

    private void Start()
    {
        // Comienza la corrutina que activa la animación cada 30 segundos
        StartCoroutine(ActivarAnimacionCadaIntervalo());
    }

    private IEnumerator ActivarAnimacionCadaIntervalo()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo); // Espera 30 segundos

            if (!isTalking)
            {
                // Activa el trigger en el Animator para reproducir la animación de hablar
                animator.SetTrigger(triggerTalking); // Activa el trigger "IsTalking"
                isTalking = true;
                Debug.Log("Animación activada: " + triggerTalking);
            }
        }
    }

    // Este método es el que llamará otro script o el sistema de daño
    public void RecibirDaño()
    {
        // Interrumpe la animación de hablar y activa la animación de daño
        animator.SetTrigger(triggerDamage); // Activa el trigger para la animación de daño
        Debug.Log("Interrumpiendo 'Hablar' debido al daño.");

        // Llamamos a una corrutina para esperar a que termine la animación de daño y luego reanudar "Hablar"
        StartCoroutine(EsperarYReanudarHablar());
    }

    private IEnumerator EsperarYReanudarHablar()
    {
        // Espera a que la animación de daño termine (por ejemplo, 1 segundo si la animación de daño dura ese tiempo)
        yield return new WaitForSeconds(1f); // Asegúrate de ajustar este tiempo según la duración de la animación de daño

        // Después de la animación de daño, reanudamos la animación de hablar
        animator.SetTrigger(triggerTalking); // Activa el trigger "IsTalking" para que vuelva a hablar
        isTalking = true;

        Debug.Log("Animación reanudada: " + triggerTalking);
    }
}