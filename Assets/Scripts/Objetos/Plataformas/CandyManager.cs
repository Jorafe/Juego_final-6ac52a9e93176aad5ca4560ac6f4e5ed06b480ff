using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class CandyManager : MonoBehaviour
{
    public TextMeshProUGUI candyText; // Texto del contador de caramelos
    public RawImage candyImage; // Imagen del HUD (con opacidad variable)

    private int candyCount = 0;
    private Color originalColor;
    private Vector3 originalSize;

    void Start()
    {
        if (candyImage != null)
        {
            originalColor = candyImage.color;
            originalSize = candyImage.rectTransform.localScale;
        }
        UpdateCandyText();
    }

    public void AddCandy()
    {
        candyCount++;
        UpdateCandyText();
        if (candyImage != null)
        {
            StopAllCoroutines();
            StartCoroutine(FlashCandyImage());
        }
    }

    private void UpdateCandyText()
    {
        if (candyText != null)
        {
            candyText.text = candyCount.ToString();
        }
    }

    // ✅ Efecto de opacidad y escala en la RawImage
    private IEnumerator FlashCandyImage()
    {
        float duration = 2f;
        float elapsed = 0f;
        Color targetColor = originalColor;
        targetColor.a = 1f; // 100% opacidad
        Vector3 targetScale = originalSize * 1.1f; // 10% más grande

        // Subir opacidad y escala
        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            candyImage.color = Color.Lerp(originalColor, targetColor, t);
            candyImage.rectTransform.localScale = Vector3.Lerp(originalSize, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        // Mantener 100% por un momento
        yield return new WaitForSeconds(0.5f);

        elapsed = 0f;
        // Bajar opacidad y escala
        while (elapsed < duration / 2)
        {
            float t = elapsed / (duration / 2);
            candyImage.color = Color.Lerp(targetColor, originalColor, t);
            candyImage.rectTransform.localScale = Vector3.Lerp(targetScale, originalSize, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Asegurar que vuelve al estado original
        candyImage.color = originalColor;
        candyImage.rectTransform.localScale = originalSize;
    }

    public int GetCandyCount()
{
    return candyCount;
}

}