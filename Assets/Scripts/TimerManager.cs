using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public GameObject pauseMenu;  // Asigna tu menú de pausa en el inspector.
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador.
    public MonoBehaviour cameraControllerScript; // Script que controla la cámara.
    public Text timerText; // Texto UI normal (Unity UI)
    public TMP_Text timerTextTMP; // Texto UI de TextMeshPro
    
    private bool isPaused = false; // Estado del juego (pausado o no).
    private float timer = 0f; // Timer en segundos
    public bool timerRunning = true; // Control del timer

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

    // Actualiza el temporizador si está corriendo
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !pauseMenu.activeSelf)
        {
            TogglePause();
        }

        // Actualizar el timer si está corriendo
        if (timerRunning)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
            
            // Mostrar el timer en el tipo de texto adecuado
            if (timerText != null)
            {
                timerText.text = timeString; // Para un texto normal (Unity UI)
            }
            if (timerTextTMP != null)
            {
                timerTextTMP.text = timeString; // Para un texto de TextMeshPro
            }
        }
    }

    // Función para detener el timer desde otro script
    public void StopTimer()
    {
        timerRunning = false;
    }

    // Función para obtener el tiempo actual
    public float GetCurrentTime()
    {
        return timer;
    }
}