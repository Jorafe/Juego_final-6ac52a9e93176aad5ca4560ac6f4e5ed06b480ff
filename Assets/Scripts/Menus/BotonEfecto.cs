using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotonEfecto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // Referencia al componente Button
    public Button boton;

    // Variables para los efectos
    private Vector3 escalaOriginal;

    // Array público para asignar los elementos (Image y RawImage) que quieres manipular
    public UnityEngine.UI.Graphic[] elementosAfectados;

    // Almacenar la opacidad original de los elementos
    private Color[] coloresOriginales;

    void Start()
    {
        // Inicializamos los componentes
        if (boton != null)
        {
            // Guardamos la escala original del botón
            escalaOriginal = boton.transform.localScale;

            // Inicializamos el array de colores originales
            coloresOriginales = new Color[elementosAfectados.Length];

            // Guardamos la opacidad original de los elementos asignados en el inspector
            for (int i = 0; i < elementosAfectados.Length; i++)
            {
                if (elementosAfectados[i] != null)
                {
                    coloresOriginales[i] = elementosAfectados[i].color;
                }
            }
        }
    }

    // Evento cuando el cursor entra en el botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Aumentar el tamaño del botón en un 2.5%
        boton.transform.localScale = escalaOriginal * 1.025f;

        // Asegurarse de que los elementos seleccionados tengan opacidad máxima
        for (int i = 0; i < elementosAfectados.Length; i++)
        {
            if (elementosAfectados[i] != null)
            {
                Color colorElemento = elementosAfectados[i].color;
                colorElemento.a = 1f; // Establecer la opacidad del elemento a 1 (totalmente opaco)
                elementosAfectados[i].color = colorElemento;
            }
        }
    }

    // Evento cuando el cursor sale del botón
    public void OnPointerExit(PointerEventData eventData)
    {
        // Restaurar el tamaño original del botón
        boton.transform.localScale = escalaOriginal;

        // Restaurar la opacidad original de los elementos seleccionados
        for (int i = 0; i < elementosAfectados.Length; i++)
        {
            if (elementosAfectados[i] != null)
            {
                elementosAfectados[i].color = coloresOriginales[i]; // Restaurar la opacidad original
            }
        }
    }
}