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

    [Header("Altura y Escala")]
    public float alturaSobreCollider = 1f;
    public Vector3 escalaEstrella = Vector3.one;

    private GameObject estrellaActual;

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

            // Tiempo de espera para volver a generar la estrella
            yield return new WaitForSeconds(tiempoEspera);
        }
    }

    private void InstanciarEstrellaAleatoria()
    {
        if (estrellaPrefab == null || baseCollider == null) return;

        Vector3 randomPoint = GetRandomPointAboveCollider(baseCollider);
        estrellaActual = Instantiate(estrellaPrefab, randomPoint, Quaternion.identity);
        estrellaActual.transform.localScale = escalaEstrella;

        // Agregar el componente que manejará la colisión y el daño a Imogen
        Estrella estrellaScript = estrellaActual.AddComponent<Estrella>();
        estrellaScript.dano = 20;  // Aquí establecemos el daño que la estrella hará a Imogen
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