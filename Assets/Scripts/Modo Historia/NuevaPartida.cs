using UnityEngine;
using UnityEngine.SceneManagement;

public class NuevaPartida : MonoBehaviour
{
    public void StartStory()
    {
        // Si ya hay un StoryManager en la escena (en modo debug, etc), no lo duplicamos
        if (StoryManager.Instance == null)
        {
            GameObject storyManagerGO = new GameObject("StoryManager");
            storyManagerGO.AddComponent<StoryManager>();
        }

        // Inicia el modo historia cargando la escena de cinemáticas
        SceneManager.LoadScene("CutsceneScene");
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteKey("storyStep");
    }
}