using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

public class NovaLive : MonoBehaviour
{
    [Range(0, 7)]
    public int vidas = 7;

    public RawImage rawImage;
    public Texture[] vidasTextures;


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

            SoundManagerMenu.Instance?.PlayPlayerDamageSFX();

            ActualizarImagenVida();

            if (vidas == 0)
            {
                Debug.Log("Nova se ha quedado sin vidas.");
                StoryManager.Instance?.LoadNextStep();
            }
        }
        else
        {
            Debug.Log("Nova ya no tiene vidas.");
            StoryManager.Instance?.LoadNextStep();
    
        }
    }

    private void ActualizarImagenVida()
    {
        if (vidasTextures != null && vidas < vidasTextures.Length)
        {
            rawImage.texture = vidasTextures[vidas];
        }
    }
    
}