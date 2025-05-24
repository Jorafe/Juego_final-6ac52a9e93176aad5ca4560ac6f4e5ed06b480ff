using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections.Generic;

public class VideoCinematicas : MonoBehaviour
{
    public RawImage videoPreview;
    public VideoPlayer videoPlayer;

    private bool isFullscreen = false;

    private Vector2 originalAnchoredPosition;
    private Vector2 originalSizeDelta;
    private Vector2 originalAnchorMin;
    private Vector2 originalAnchorMax;
    private Vector2 originalPivot;

    private static List<VideoCinematicas> allVideos = new List<VideoCinematicas>();
    private List<GameObject> hijosDesactivados = new List<GameObject>();

    void Awake()
    {
        allVideos.Add(this);
    }

    void OnDestroy()
    {
        allVideos.Remove(this);
    }

    void Start()
    {
        videoPlayer.isLooping = false;
        videoPlayer.loopPointReached += OnVideoEnd;
        videoPlayer.Stop();

        RectTransform rt = videoPreview.rectTransform;
        originalAnchoredPosition = rt.anchoredPosition;
        originalSizeDelta = rt.sizeDelta;
        originalAnchorMin = rt.anchorMin;
        originalAnchorMax = rt.anchorMax;
        originalPivot = rt.pivot;
    }

    void OnDisable()
    {
        videoPlayer.loopPointReached -= OnVideoEnd;
    }

    public void OnClickVideo()
    {
        if (!isFullscreen)
        {
            DesactivarOtrosVideos();
            DesactivarHijos();
            SetToFullScreen();
            videoPlayer.Play();
            SoundManagerMenu.Instance?.StartCinematic();
        }
        else
        {
            RestaurarOtrosVideos();
            ActivarHijos();
            RestoreOriginal();
            videoPlayer.Stop();
            SoundManagerMenu.Instance?.EndCinematic();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        RestaurarOtrosVideos();
        ActivarHijos();
        RestoreOriginal();
        videoPlayer.Stop();
        SoundManagerMenu.Instance?.EndCinematic();
    }

    void SetToFullScreen()
    {
        isFullscreen = true;
        RectTransform rt = videoPreview.rectTransform;

        LayoutElement le = videoPreview.GetComponent<LayoutElement>();
        if (le) Destroy(le);

        rt.anchorMin = new Vector2(0, 0);
        rt.anchorMax = new Vector2(0, 0);
        rt.pivot = new Vector2(0.5f, 0.5f);

        rt.anchoredPosition = new Vector2(Screen.width / 2f, Screen.height / 2f);
        rt.sizeDelta = new Vector2(1920, 1080);
    }

    void RestoreOriginal()
    {
        isFullscreen = false;
        RectTransform rt = videoPreview.rectTransform;

        rt.anchorMin = originalAnchorMin;
        rt.anchorMax = originalAnchorMax;
        rt.pivot = originalPivot;
        rt.anchoredPosition = originalAnchoredPosition;
        rt.sizeDelta = originalSizeDelta;
    }

    void DesactivarOtrosVideos()
    {
        foreach (var video in allVideos)
        {
            if (video != this)
                video.gameObject.SetActive(false);
        }
    }

    void RestaurarOtrosVideos()
    {
        foreach (var video in allVideos)
        {
            if (video != this)
                video.gameObject.SetActive(true);
        }
    }

    void DesactivarHijos()
    {
        hijosDesactivados.Clear();
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
                hijosDesactivados.Add(child.gameObject);
            }
        }
    }

    void ActivarHijos()
    {
        foreach (var go in hijosDesactivados)
        {
            if (go != null)
                go.SetActive(true);
        }
        hijosDesactivados.Clear();
    }
}