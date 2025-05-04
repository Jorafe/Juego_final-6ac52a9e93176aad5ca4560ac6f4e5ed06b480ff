using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCinematicas : MonoBehaviour
{
    public RawImage videoPreview;
    public RectTransform fullScreenRect;
    public VideoPlayer videoPlayer;

    private bool isFullscreen = false;

    // Variables para guardar la posición y tamaño originales
    private Vector3 originalPosition;
    private Vector2 originalSize;

    void Start()
    {
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Stop();

        // Guardar valores originales
        originalPosition = videoPreview.rectTransform.position;
        originalSize = videoPreview.rectTransform.sizeDelta;
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    public void OnClickVideo()
    {
        if (!isFullscreen)
        {
            SetToFullScreen();
            videoPlayer.Play();
        }
        else
        {
            RestoreOriginal();
            videoPlayer.Stop();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        RestoreOriginal();
        videoPlayer.Stop();
    }

    void SetToFullScreen()
    {
        isFullscreen = true;
        videoPreview.rectTransform.position = fullScreenRect.position;
        videoPreview.rectTransform.sizeDelta = fullScreenRect.sizeDelta;
    }

    void RestoreOriginal()
    {
        isFullscreen = false;
        videoPreview.rectTransform.position = originalPosition;
        videoPreview.rectTransform.sizeDelta = originalSize;
    }
}