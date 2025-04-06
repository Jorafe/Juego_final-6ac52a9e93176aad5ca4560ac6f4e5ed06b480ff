using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BotonEfecto : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Button boton;
    private Vector3 escalaOriginal;
    public UnityEngine.UI.Graphic[] elementosAfectados;
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

            // Añadir el sonido al evento OnClick
            boton.onClick.AddListener(() =>
            {
                if (SoundManagerMenu.Instance != null)
                {
                    SoundManagerMenu.Instance.PlaySFX(SoundManagerMenu.Instance.clickSFX);
                }
            });
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        boton.transform.localScale = escalaOriginal * 1.025f;

        for (int i = 0; i < elementosAfectados.Length; i++)
        {
            if (elementosAfectados[i] != null)
            {
                Color colorElemento = elementosAfectados[i].color;
                colorElemento.a = 1f;
                elementosAfectados[i].color = colorElemento;
            }
        }

        // Reproducir sonido hover
        if (SoundManagerMenu.Instance != null)
        {
            SoundManagerMenu.Instance.PlaySFX(SoundManagerMenu.Instance.hoverSFX);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
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
}