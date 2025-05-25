using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinuarModoHistoria : MonoBehaviour
{
    public void OnContinuePressed()
    {
        StoryManager.Instance.LoadNextStep();
    }

    public void OnRetryPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    public void OnMainMenuPressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
