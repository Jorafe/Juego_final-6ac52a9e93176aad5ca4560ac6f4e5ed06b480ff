using UnityEngine;
using UnityEngine.UI;

public class Bomba : MonoBehaviour
{
    [SerializeField] private Canvas canvas; // Asigna el Canvas manualmente
    [SerializeField] private Button button; // Asigna el botón manualmente

    void Start()
    {
        // Verifica si el botón está asignado
        if (button == null)
        {
            Debug.LogError("Bomba: No se encontró un botón. Asigna uno manualmente en el inspector.");
            return;
        }

        // Asigna la función al botón
        button.onClick.AddListener(Explode);
    }

    void Explode()
    {
        if (canvas != null)
        {
            // Llama al BombaManager para iniciar la destrucción
            BombaManager.Instance.DestroyCanvasAfterDelay(canvas.gameObject, 3f);
        }
    }
}