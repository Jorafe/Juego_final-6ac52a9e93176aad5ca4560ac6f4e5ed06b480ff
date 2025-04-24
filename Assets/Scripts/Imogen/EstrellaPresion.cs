using System.Collections;
using UnityEngine;

public class EstrellaPresion : MonoBehaviour
{
    [Header("Prefab y Objeto Base")]
    public GameObject estrellaPrefab;
    public Collider baseCollider;

    [Header("Parámetros de Tiempo")]
    public float tiempoInicio = 15f;
    public float duracionVisible = 7f;
    public float tiempoEspera = 3f;
    public float tiempoEsperaTrasJugador = 10f;
    public float delayDesaparicionJugador = 1f;

    [Header("Altura y Escala")]
    public float alturaSobreCollider = 1f;
    public Vector3 escalaEstrella = Vector3.one;

    private GameObject estrellaActual;
    private bool fueTocadaPorJugador = false;

    private void Start()
    {
        StartCoroutine(CicloEstrella());
    }

    private IEnumerator CicloEstrella()
    {
        yield return new WaitForSeconds(tiempoInicio);

        while (true)
        {
            InstanciarEstrellaAleatoria();
            fueTocadaPorJugador = false;

            float tiempoRestante = duracionVisible;
            while (estrellaActual != null && tiempoRestante > 0f && !fueTocadaPorJugador)
            {
                tiempoRestante -= Time.deltaTime;
                yield return null;
            }

            if (estrellaActual != null)
            {
                if (fueTocadaPorJugador)
                {
                    yield return new WaitForSeconds(delayDesaparicionJugador);
                }

                Destroy(estrellaActual);
            }

            float tiempoEsperaActual = fueTocadaPorJugador ? tiempoEsperaTrasJugador : tiempoEspera;
            yield return new WaitForSeconds(tiempoEsperaActual);
        }
    }

    private void InstanciarEstrellaAleatoria()
    {
        if (estrellaPrefab == null || baseCollider == null) return;

        Vector3 randomPoint = GetRandomPointAboveCollider(baseCollider);
        estrellaActual = Instantiate(estrellaPrefab, randomPoint, Quaternion.identity);
        estrellaActual.transform.localScale = escalaEstrella;

        EstrellaCollision estrellaScript = estrellaActual.AddComponent<EstrellaCollision>();
        estrellaScript.presionManager = this;
    }

    private Vector3 GetRandomPointAboveCollider(Collider col)
    {
        Bounds bounds = col.bounds;

        float x = Random.Range(bounds.min.x, bounds.max.x);
        float z = Random.Range(bounds.min.z, bounds.max.z);
        float y = bounds.max.y + alturaSobreCollider;

        return new Vector3(x, y, z);
    }

    // Este método lo llama el otro script cuando el Player toca la estrella
    public void MarcarComoTocadaPorJugador()
    {
        fueTocadaPorJugador = true;
    }
}