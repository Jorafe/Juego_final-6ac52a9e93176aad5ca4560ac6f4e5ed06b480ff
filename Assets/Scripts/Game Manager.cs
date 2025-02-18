using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public GameObject pauseMenu;  // Asigna tu menú de pausa en el inspector.
    private bool isPaused = false; // Estado del juego (en pausa o no).

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

    // Esta función se llama para alternar entre pausa y reanudación del juego
    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Detiene el tiempo
            pauseMenu.SetActive(true); // Muestra el menú de pausa
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el tiempo
            pauseMenu.SetActive(false); // Oculta el menú de pausa
        }
    }

    // Detecta si se presionó la tecla Escape
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

}