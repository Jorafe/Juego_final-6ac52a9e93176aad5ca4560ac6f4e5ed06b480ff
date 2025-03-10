using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombaManager : MonoBehaviour
{
    public static BombaManager Instance; // Singleton para acceder desde cualquier script

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DestroyCanvasAfterDelay(GameObject canvas, float delay)
    {
        StartCoroutine(DestroyCanvasCoroutine(canvas, delay));
    }

    private IEnumerator DestroyCanvasCoroutine(GameObject canvas, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (canvas != null)
        {
            Destroy(canvas);
        }
    }
}
