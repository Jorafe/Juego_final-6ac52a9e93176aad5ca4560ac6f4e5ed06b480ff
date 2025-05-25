using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotonEfecto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    public Button boton;
    private Vector3 escalaOriginal;
    public Graphic[] elementosAfectados;
    private Color[] coloresOriginales;

    void Start()
    {
        if (boton != null)
        {
            escalaOriginal = boton.transform.localScale;

            coloresOriginales = new Color[elementosAfectados.Length];
            for (int i = 0; i < elementosAfectados.Length; i++)
            {
                if (elementosAfectados[i] != null)
                {
                    coloresOriginales[i] = elementosAfectados[i].color;
                }
            }

            // Añadir sonido al hacer clic
            boton.onClick.AddListener(() =>
            {
                if (SoundManagerMenu.Instance != null)
                {
                    SoundManagerMenu.Instance.PlaySFX(SoundManagerMenu.Instance.clickSFX);
                }
            });
        }
    }

    // Se llama cuando el ratón entra o cuando se selecciona con el joystick/teclado
    private void ActivarEfecto()
    {
        boton.transform.localScale = escalaOriginal * 1.025f;

        for (int i = 0; i < elementosAfectados.Length; i++)
        {
            if (elementosAfectados[i] != null)
            {
                Color c = elementosAfectados[i].color;
                c.a = 1f;
                elementosAfectados[i].color = c;
            }
        }

        if (SoundManagerMenu.Instance != null)
        {
            SoundManagerMenu.Instance.PlaySFX(SoundManagerMenu.Instance.hoverSFX);
        }
    }

    // Se llama cuando el ratón sale o se pierde la selección con joystick/teclado
    private void DesactivarEfecto()
    {
        boton.transform.localScale = escalaOriginal;

        for (int i = 0; i < elementosAfectados.Length; i++)
        {
            if (elementosAfectados[i] != null)
            {
                elementosAfectados[i].color = coloresOriginales[i];
            }
        }
    }

    // Eventos para ratón
    public void OnPointerEnter(PointerEventData eventData) => ActivarEfecto();
    public void OnPointerExit(PointerEventData eventData) => DesactivarEfecto();

    // Eventos para joystick/teclado
    public void OnSelect(BaseEventData eventData) => ActivarEfecto();
    public void OnDeselect(BaseEventData eventData) => DesactivarEfecto();
}