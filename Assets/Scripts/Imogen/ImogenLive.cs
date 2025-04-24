using UnityEngine;
using System.Collections;

public class ImogenLive : MonoBehaviour
{
    [Header("Vidas")]
    public int maxLives = 100;
    private int currentLives;

    public string bulletTag = "whatisBullet";

    private void Start()
    {
        currentLives = maxLives;
        Debug.Log("Vidas iniciales: " + currentLives);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(bulletTag))
        {
            TakeDamage(1);
        }
        else if (collision.gameObject.CompareTag("whatisEstrella") && collision.collider.CompareTag("Player"))
        {
            StartCoroutine(DelayedDamage(10, 3f)); // 10 de daño tras 3 segundos
        }
    }

    private IEnumerator DelayedDamage(int damage, float delay)
    {
        yield return new WaitForSeconds(delay);
        TakeDamage(damage);
    }

    private void TakeDamage(int amount)
    {
        currentLives -= amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        Debug.Log("Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            Debug.Log("¡Se han agotado las vidas de Imogen!");
            // Aquí puedes añadir lógica de muerte si lo deseas.
        }
    }
}