using UnityEngine;

public class ImogenLive : MonoBehaviour
{
    [Header("Vidas")]
    public int maxLives = 30;
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
    }

    private void TakeDamage(int amount)
    {
        currentLives -= amount;
        currentLives = Mathf.Clamp(currentLives, 0, maxLives);

        // Mostrar el número de vidas restantes en el log
        Debug.Log("Vidas restantes: " + currentLives);

        if (currentLives <= 0)
        {
            Debug.Log("¡Se han agotado las vidas de Imogen!");
            // Aquí puedes añadir lógica de muerte si lo deseas.
        }
    }
}