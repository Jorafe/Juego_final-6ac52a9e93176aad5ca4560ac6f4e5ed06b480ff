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

    [Header("Cinemática de Muerte")]
    [SerializeField] private VideoPlayer videoCinematica;

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
                if (videoCinematica != null)
                {
                    StartCoroutine(ReproducirCinematicaTrasRetraso(3f));
                }
            }
        }
        else
        {
            Debug.Log("Nova ya no tiene vidas.");
        }
    }

    private void ActualizarImagenVida()
    {
        if (vidasTextures != null && vidas < vidasTextures.Length)
        {
            rawImage.texture = vidasTextures[vidas];
        }
    }

    private IEnumerator ReproducirCinematicaTrasRetraso(float segundos)
    {
        yield return new WaitForSeconds(segundos);
        videoCinematica.gameObject.SetActive(true);
        videoCinematica.Play();
    }
}