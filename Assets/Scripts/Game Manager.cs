using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;  // Menú de pausa asignado en el inspector.
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador.
    public MonoBehaviour cameraControllerScript; // Script que controla la cámara (solo desactiva el movimiento).
    
    private bool isPaused = false; // Estado del juego (pausado o no).

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pausa el tiempo
            pauseMenu.SetActive(true); // Muestra el menú de pausa
            
            // Desactiva los controles del jugador y la cámara
            if (playerMovementScript != null) playerMovementScript.enabled = false;
            if (cameraControllerScript != null) cameraControllerScript.enabled = false;

            Cursor.lockState = CursorLockMode.None; // Libera el cursor
            Cursor.visible = true; // Hace visible el cursor
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el tiempo
            pauseMenu.SetActive(false); // Oculta el menú de pausa
            
            // Reactiva los controles del jugador y la cámara
            if (playerMovementScript != null) playerMovementScript.enabled = true;
            if (cameraControllerScript != null) cameraControllerScript.enabled = true;

            Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
            Cursor.visible = false; // Oculta el cursor
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }
}
