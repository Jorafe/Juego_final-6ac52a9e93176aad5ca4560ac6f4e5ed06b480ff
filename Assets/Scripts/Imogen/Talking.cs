using UnityEngine;
using System.Collections;
using TMPro;

public class Talking : MonoBehaviour
{
    [Header("Animación")]
    public Animator animator;
    public string triggerTalking = "IsTalking";
    public string triggerDamage = "TakeDamage";

    [Header("Parámetros de tiempo")]
    public float intervalo = 30f;
    public float duracionAnimacionHablar = 3f;

    [Header("Frases")]
    public string[] frases;
    public TextMeshProUGUI textoFrase;

    private bool isTalking = false;

    private void Start()
    {
        StartCoroutine(ActivarAnimacionCadaIntervalo());
    }

    private IEnumerator ActivarAnimacionCadaIntervalo()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            if (!isTalking)
            {
                animator.SetTrigger(triggerTalking);
                isTalking = true;
                Debug.Log("Animación activada: " + triggerTalking);
                MostrarFraseAleatoria(); // Solo la muestra si isTalking es true
                StartCoroutine(EsperarFinHablar());
            }
        }
    }

    public void RecibirDaño()
    {
        animator.SetTrigger(triggerDamage);
        Debug.Log("Interrumpiendo 'Hablar' debido al daño.");
        StartCoroutine(EsperarYReanudarHablar());
    }

    private IEnumerator EsperarYReanudarHablar()
    {
        yield return new WaitForSeconds(1f);

        animator.SetTrigger(triggerTalking);
        isTalking = true;
        Debug.Log("Animación reanudada: " + triggerTalking);
        MostrarFraseAleatoria();
        StartCoroutine(EsperarFinHablar());
    }

    private void MostrarFraseAleatoria()
    {
        if (!isTalking || frases.Length == 0 || textoFrase == null) return;

        textoFrase.text = frases[Random.Range(0, frases.Length)];
        textoFrase.gameObject.SetActive(true);
    }

    private IEnumerator EsperarFinHablar()
    {
        yield return new WaitForSeconds(duracionAnimacionHablar);

        if (textoFrase != null)
            textoFrase.gameObject.SetActive(false);

        isTalking = false;
    }
}