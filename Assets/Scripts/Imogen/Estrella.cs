using UnityEngine;
using System.Collections;

public class Estrella : MonoBehaviour
{
    [Header("Dano")]
    public int dano = 20;
    public GameObject enemigoGO; 
    private ImogenLive enemigo;
    private bool yaCo = false;

    private void Start()
    {
        if (enemigoGO != null)
        {
            enemigo = enemigoGO.GetComponent<ImogenLive>();
            if (enemigo == null)
            {
                Debug.LogError("El GameObject asignado no tiene el componente ImogenLive.");
            }
        }
        else
        {
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if (yaCo) return;

        if (other.CompareTag("Player"))
        {
            yaCo = true;

            if (enemigo != null)
            {
                enemigo.RecibirDaño(dano);
                Debug.Log($"¡La estrella hizo {dano} de dano a Imogen! Vidas restantes: {enemigo.VidasActuales}");
            }

            StartCoroutine(DesactivarDespuesDeTiempo());
        }
    }

    private IEnumerator DesactivarDespuesDeTiempo()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }
}