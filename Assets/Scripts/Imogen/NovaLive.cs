using UnityEngine;
using UnityEngine.UI;

public class NovaLive : MonoBehaviour
{
    [Range(0, 7)]
    public int vidas = 7;

    public RawImage rawImage; // El RawImage que se mostrará en el UI
    public Texture[] vidasTextures; // Las 8 texturas (de 0 a 7 vidas)

    private void Start()
    {
        ActualizarImagenVida();
    }

    public void QuitarVida()
    {
        if (vidas > 0)
        {
            vidas--;
            Debug.Log("Vida perdida. Vidas restantes: " + vidas);
            ActualizarImagenVida();
        }
        else
        {
            Debug.Log("Nova ya no tiene vidas.");
        }
    }

    private void ActualizarImagenVida()
    {
        if (vidasTextures != null && vidasTextures.Length > vidas)
        {
            rawImage.texture = vidasTextures[vidas];
        }
    }
}