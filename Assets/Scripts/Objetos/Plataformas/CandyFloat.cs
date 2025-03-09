using UnityEngine;
using System.Collections;

public class CandyFloat : MonoBehaviour
{
    public float scaleFactor = 1.05f;  // Escala al 105%
    public float moveDistance = 1f;    // Sube 1 metro
    public float duration = 2f;        // Duración más lenta y suave

    private Vector3 originalScale;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Start()
    {
        originalScale = transform.localScale;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        StartCoroutine(ScaleLoop());
        StartCoroutine(MoveLoop());
        StartCoroutine(RotateLoop());
    }

    IEnumerator ScaleLoop()
    {
        while (true)
        {
            yield return SmoothLerpScale(originalScale * scaleFactor, duration / 2);
            yield return SmoothLerpScale(originalScale, duration / 2);
        }
    }

    IEnumerator MoveLoop()
    {
        while (true)
        {
            yield return SmoothLerpMove(originalPosition.y + moveDistance, duration);
            yield return SmoothLerpMove(originalPosition.y, duration);
        }
    }

    IEnumerator RotateLoop()
    {
        while (true)
        {
            Quaternion targetRotation = Random.rotation; // Genera una rotación aleatoria
            yield return SmoothLerpRotation(targetRotation, duration * 2);
        }
    }

    IEnumerator SmoothLerpScale(Vector3 targetScale, float time)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0;
        while (elapsed < time)
        {
            float t = (1 - Mathf.Cos((elapsed / time) * Mathf.PI)) / 2; // Hace la transición más suave
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localScale = targetScale;
    }

    IEnumerator SmoothLerpMove(float targetY, float time)
    {
        Vector3 startPos = transform.position;
        Vector3 targetPos = new Vector3(startPos.x, targetY, startPos.z);
        float elapsed = 0;
        while (elapsed < time)
        {
            float t = (1 - Mathf.Cos((elapsed / time) * Mathf.PI)) / 2; // Movimiento más suave
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = targetPos;
    }

    IEnumerator SmoothLerpRotation(Quaternion targetRotation, float time)
    {
        Quaternion startRotation = transform.rotation;
        float elapsed = 0;
        while (elapsed < time)
        {
            float t = (1 - Mathf.Cos((elapsed / time) * Mathf.PI)) / 2; // Suaviza la rotación
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = targetRotation;
    }

    // ✅ Detecta si el Player toca el SphereCollider y destruye el caramelo
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
    {
        CandyManager candyManager = FindObjectOfType<CandyManager>();
        if (candyManager != null)
        {
            candyManager.AddCandy(); // ✅ Suma caramelos
        }
        Destroy(gameObject); // ✅ Elimina el caramelo
    }
    }
}

    