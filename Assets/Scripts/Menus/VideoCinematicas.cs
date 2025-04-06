using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoCinematicas : MonoBehaviour
{
    public RawImage videoPreview;              // Pequeño tamaño
    public RectTransform fullScreenRect;       // Tamaño a pantalla completa
    public RectTransform smallRect;            // Tamaño pequeño
    public VideoPlayer videoPlayer;

    private bool isFullscreen = false;

    void Start()
    {
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Stop();
        SetToSmall();
    }

    public void OnClickVideo()
    {
        if (!isFullscreen)
        {
            SetToFullScreen();
            videoPlayer.Play();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        SetToSmall();
        videoPlayer.Stop();
    }

    void SetToFullScreen()
    {
        isFullscreen = true;
        videoPreview.rectTransform.position = fullScreenRect.position;
        videoPreview.rectTransform.sizeDelta = fullScreenRect.sizeDelta;
    }

    void SetToSmall()
    {
        isFullscreen = false;
        videoPreview.rectTransform.position = smallRect.position;
        videoPreview.rectTransform.sizeDelta = smallRect.sizeDelta;
    }
}
