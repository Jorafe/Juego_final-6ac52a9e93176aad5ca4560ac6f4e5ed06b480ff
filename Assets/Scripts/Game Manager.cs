using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject pauseMenu;  // Asigna tu menú de pausa en el inspector.
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador.
    public MonoBehaviour cameraControllerScript; // Script que controla la cámara.
    public TMP_Text timerText; // Texto UI para mostrar el timer con TextMeshPro
    
    private bool isPaused = false; // Estado del juego (pausado o no).
    private float timer = 0f; // Timer en segundos
    public bool timerRunning = true; // Control del timer

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
            Time.timeScale = 0f; // Pausa el tiempo
            pauseMenu.SetActive(true); // Muestra el menú de pausa
            
            if (playerMovementScript != null) playerMovementScript.enabled = false; // Desactiva el movimiento
            if (cameraControllerScript != null) cameraControllerScript.enabled = false; // Desactiva la cámara
            
            Cursor.lockState = CursorLockMode.None; // Libera el cursor
            Cursor.visible = true; // Hace visible el cursor
            
            timerRunning = false; // Pausar el timer
        }
        else
        {
            Time.timeScale = 1f; // Reanuda el tiempo
            pauseMenu.SetActive(false); // Oculta el menú de pausa
            
            if (playerMovementScript != null) playerMovementScript.enabled = true; // Reactiva el movimiento
            if (cameraControllerScript != null) cameraControllerScript.enabled = true; // Reactiva la cámara
            
            Cursor.lockState = CursorLockMode.Locked; // Bloquea el cursor
            Cursor.visible = false; // Oculta el cursor
            
            timerRunning = true; // Reanudar el timer
        }
    }

    // Detecta si se presionó la tecla Escape
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // Actualizar el timer si está corriendo
        if (timerRunning)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Función para detener el timer desde otro script
    public void StopTimer()
    {
        timerRunning = false;
    }
}

