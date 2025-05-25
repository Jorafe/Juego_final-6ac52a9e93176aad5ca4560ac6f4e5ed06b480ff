using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneController : MonoBehaviour
{
    [Tooltip("Arrastra aquí todos los GameObjects que contienen VideoPlayers, en orden.")]
    public List<GameObject> cutsceneObjects;

    void Start()
    {
        int index = StoryManager.Instance.GetCurrentCutsceneIndex();

        if (index >= cutsceneObjects.Count)
        {
            Debug.LogError("El índice de cinemática está fuera del rango.");
            return;
        }

        // Activar solo la cinemática correspondiente
        for (int i = 0; i < cutsceneObjects.Count; i++)
            cutsceneObjects[i].SetActive(i == index);

        GameObject currentCutscene = cutsceneObjects[index];
        VideoPlayer vp = currentCutscene.GetComponent<VideoPlayer>();

        if (vp == null)
        {
            Debug.LogError("El GameObject no tiene un VideoPlayer asignado.");
            return;
        }

        vp.loopPointReached += OnVideoFinished;
        vp.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        vp.loopPointReached -= OnVideoFinished;
        StoryManager.Instance.LoadNextStep();
    }
}