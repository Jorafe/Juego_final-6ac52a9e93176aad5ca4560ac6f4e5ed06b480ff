using UnityEngine;
using TMPro;
using System.Collections;  // Asegúrate de incluir esta línea para usar IEnumerator

public class CandyShoot : MonoBehaviour
{
    public static CandyShoot Instance;  // Instancia del GameManager

    public int totalCandies = 0;  // Total de caramelos acumulados
    public TextMeshProUGUI candyCountText;  // Texto en el HUD para mostrar los caramelos

    private void Awake()
    {
        // Asegurarse de que solo exista una instancia de CandyShoot
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Si hay más de una instancia, destruirla
        }
    }

    public void AddCandies(int amount)
    {
        totalCandies += amount;  // Sumar los caramelos
        UpdateCandyDisplay();  // Actualizar la visualización del contador de caramelos
    }

    private void UpdateCandyDisplay()
    {
        if (candyCountText != null)
        {
            candyCountText.text = totalCandies.ToString();  // Mostrar el total de caramelos en el HUD
        }
    }

    // Corutina para animar el texto del número de caramelos de 0 a la cantidad final
    public IEnumerator AnimateCandyText(int targetCandy, TextMeshProUGUI candyText)
    {
        float currentCandy = 0;
        float duration = 1f;  // Duración de la animación en segundos
        float elapsedTime = 0;

        // Sumar los caramelos poco a poco
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            currentCandy = Mathf.Lerp(0, targetCandy, elapsedTime / duration);
            candyText.text = Mathf.FloorToInt(currentCandy).ToString();
            yield return null;
        }

        candyText.text = targetCandy.ToString();  // Asegurarse de que el texto llegue al valor final
    }
}