using System.Collections;
using UnityEngine;
using TMPro;

public class EstrellaPresion : MonoBehaviour
{
    [Header("Prefab y Objeto Base")]
    public GameObject estrellaPrefab;
    public Collider baseCollider;

    [Header("Parámetros de Tiempo")]
    public float tiempoInicio = 15f;
    public float duracionVisible = 7f;
    public float tiempoEspera = 3f;

    [Header("Altura y Escala")]
    public float alturaSobreCollider = 1f;
    public Vector3 escalaEstrella = Vector3.one;

    [Header("UI")]
    public TextMeshProUGUI mensajeTexto; // Asignar en el inspector
    public string mensaje = "¡Ha aparecido una estrella!";

    private GameObject estrellaActual;

    private void Start()
    {
        if (mensajeTexto != null)
            mensajeTexto.gameObject.SetActive(false);

        StartCoroutine(CicloEstrella());
    }

    private IEnumerator CicloEstrella()
    {
        yield return new WaitForSeconds(tiempoInicio);

        while (true)
        {
            InstanciarEstrellaAleatoria();
            StartCoroutine(MostrarMensajeTemporal());

            float tiempoRestante = duracionVisible;
            while (estrellaActual != null && tiempoRestante > 0f)
            {
                tiempoRestante -= Time.deltaTime;
                yield return null;
            }

            if (estrellaActual != null)
            {
                Destroy(estrellaActual);
            }

            yield return new WaitForSeconds(tiempoEspera);
        }
    }

    private void InstanciarEstrellaAleatoria()
    {
        if (estrellaPrefab == null || baseCollider == null) return;

        Vector3 randomPoint = GetRandomPointAboveCollider(baseCollider);
        estrellaActual = Instantiate(estrellaPrefab, randomPoint, Quaternion.identity);
        estrellaActual.transform.localScale = escalaEstrella;

        Estrella estrellaScript = estrellaActual.AddComponent<Estrella>();
        estrellaScript.dano = 20;
    }

    private IEnumerator MostrarMensajeTemporal()
    {
        if (mensajeTexto == null) yield break;

        mensajeTexto.text = mensaje;
        mensajeTexto.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        mensajeTexto.gameObject.SetActive(false);
    }

    private Vector3 GetRandomPointAboveCollider(Collider col)
    {
        Bounds bounds = col.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y + alturaSobreCollider;

        return new Vector3(x, y, z);
    }
}