using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
   public GameObject canvas;  // El Canvas que se quiere desactivar

    void Start()
    {
        // Asegúrate de que el Canvas está activo al inicio
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    void Update()
    {
        // Comprobamos si se presiona cualquier tecla
        if (Input.anyKeyDown)
        {
            // Desactivamos el Canvas permanentemente
            if (canvas != null)
            {
                canvas.SetActive(false);
            }
        }
    }
}
