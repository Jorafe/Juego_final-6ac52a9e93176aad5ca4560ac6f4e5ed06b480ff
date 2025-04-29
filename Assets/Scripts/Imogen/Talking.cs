using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Talking : MonoBehaviour
{
    [Header("Frases")]
    public string[] frasesTexto;            // Frases escritas que se mostrarán
    public Text textoUI;                    // Elemento UI de tipo Text para mostrar frases

    [Header("Animación")]
    public Animator animador;               // Animator que se usará para la animación
    public string nombreTriggerAnimacion = "Hablar";  // Nombre del trigger en el Animator

    [Header("Parámetros")]
    public float intervalo = 30f;           // Tiempo entre cada frase
    public float duracionTexto = 5f;        // Cuánto tiempo se muestra la frase

    private int indiceActual = 0;

    private void Start()
    {
        if (frasesTexto.Length == 0)
        {
            Debug.LogWarning("No se han asignado frases.");
            return;
        }

        if (textoUI != null)
            textoUI.enabled = false;

        StartCoroutine(ReproducirFrases());
    }

    private IEnumerator ReproducirFrases()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            // Activar animación
            if (animador != null && !string.IsNullOrEmpty(nombreTriggerAnimacion))
            {
                animador.SetTrigger(nombreTriggerAnimacion);
            }

            // Mostrar texto
            if (textoUI != null)
            {
                textoUI.text = frasesTexto[indiceActual];
                textoUI.enabled = true;

                yield return new WaitForSeconds(duracionTexto);

                textoUI.enabled = false;
            }

            // Pasar a la siguiente frase (volver al principio si se termina)
            indiceActual = (indiceActual + 1) % frasesTexto.Length;
        }
    }
}
