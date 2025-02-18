using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public void LoadScene(string Scene)
    {
        SceneManager.LoadScene(Scene);
    }
    

    public void DisableCanvas(string currentCanvas)
    {
        GameObject canvas = GameObject.Find(currentCanvas);
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    public void EnableCanvas(GameObject newCanvas)
    {
        if (newCanvas != null)
        {
            newCanvas.SetActive(true);
        }
    }
}

