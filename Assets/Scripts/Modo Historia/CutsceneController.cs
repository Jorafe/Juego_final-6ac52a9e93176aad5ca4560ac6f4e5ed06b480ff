using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    public List<PlayableDirector> cutscenes;

    void Start()
    {
        int index = StoryManager.Instance.GetCurrentCutsceneIndex();
        if (index >= cutscenes.Count) {
            Debug.LogError("Cutscene index fuera de rango");
            return;
        }

        cutscenes[index].gameObject.SetActive(true);
        cutscenes[index].Play();
        cutscenes[index].stopped += OnCutsceneFinished;
    }

    void OnCutsceneFinished(PlayableDirector dir)
    {
        dir.stopped -= OnCutsceneFinished;
        StoryManager.Instance.LoadNextStep();
    }
}
