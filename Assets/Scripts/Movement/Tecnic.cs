using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class Tecnic : MonoBehaviour
{
    private TimerManager timeManager;
    private CandyManager candyManager;  // Referencia al CandyManager

    public GameObject endGameCanvas; // El Canvas que aparecerá al tocar la zona final
    public Text victoryTimeText; // El texto normal (Unity UI) donde se mostrará el tiempo
    public TMP_Text victoryTimeTMPText; // El TMP_Text donde se mostrará el tiempo en el canvas de victoria

    public Text victoryCandyText; // El texto normal (Unity UI) donde se mostrará el número de caramelos
    public TMP_Text victoryCandyTMPText; // El TMP_Text donde se mostrará el número de caramelos en el canvas de victoria

    public GameObject player; // El GameObject del jugador (asignado en el inspector)
    public GameObject pauseMenu; // El menú de pausa (asignado en el inspector)
    public MonoBehaviour playerMovementScript; // Script de movimiento del jugador
    public MonoBehaviour cameraControllerScript; // Script de control de la cámara

    void Start()
    {
        timeManager = FindObjectOfType<TimerManager>();
        candyManager = FindObjectOfType<CandyManager>();  // Encuentra el CandyManager en la escena
    }

    private void Update()
    {
        // Solo permite abrir el menú de pausa si el menú de victoria no está activo
        if (Input.GetKeyDown(KeyCode.Escape) && !endGameCanvas.activeSelf)
        {
            TogglePause();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si el objeto que entra en el trigger tiene la capa "WhatIsTimer"
        if (other.gameObject.layer == LayerMask.NameToLayer("WhatIsTimer"))
        {
            // Si se encuentra el TimerManager, se detiene el temporizador
            if (timeManager != null)
            {
                timeManager.StopTimer();
            }

            // Llama a la corutina para esperar 1 segundo antes de desactivar el jugador y mostrar el canvas
            StartCoroutine(HandleVictory());
        }
    }

    // Corutina para esperar 1 segundo antes de activar el menú de victoria y desactivar al jugador
    private IEnumerator HandleVictory()
    {
        // Espera 1 segundo antes de continuar
        yield return new WaitForSeconds(1f);

        // Muestra el canvas de victoria
        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(true);
        }

        // Desactiva el menú de pausa si está activo
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
        }

        // Desactiva el GameObject del jugador (evita que el jugador se mueva)
        if (player != null)
        {
            player.SetActive(false); // Desactiva el GameObject del jugador
        }

        // Desactiva el movimiento y la cámara
        if (playerMovementScript != null) playerMovementScript.enabled = false;
        if (cameraControllerScript != null) cameraControllerScript.enabled = false;

        // Mostrar el cursor
        Cursor.lockState = CursorLockMode.None; // Libera el cursor
        Cursor.visible = true; // Hace visible el cursor

        // Obtener el tiempo actual del TimerManager
        if (timeManager != null)
        {
            float finalTime = timeManager.GetCurrentTime();
            int minutes = Mathf.FloorToInt(finalTime / 60);
            int seconds = Mathf.FloorToInt(finalTime % 60);
            string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);

            // Asignar solo los números (sin texto adicional)
            if (victoryTimeText != null)
            {
                victoryTimeText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Solo los números
            }
            if (victoryTimeTMPText != null)
            {
                victoryTimeTMPText.text = string.Format("{0:00}:{1:00}", minutes, seconds); // Solo los números
            }
        }

        // Obtener el total de caramelos recogidos
        if (candyManager != null)
        {
            int candyCount = candyManager.GetCandyCount();

            // Asignar solo los números (sin texto adicional)
            if (victoryCandyText != null)
            {
                victoryCandyText.text = candyCount.ToString(); // Solo el número
            }
            if (victoryCandyTMPText != null)
            {
                victoryCandyTMPText.text = candyCount.ToString(); // Solo el número
            }
        }
    }

    // Función para alternar entre pausa y reanudación del juego
    public void TogglePause()
    {
        if (endGameCanvas.activeSelf) return; // Si el menú de victoria está activo, no permite pausar

        bool isPaused = Time.timeScale == 0f;
        if (isPaused)
        {
            Time.timeScale = 1f;
            playerMovementScript.enabled = true;
            cameraControllerScript.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Time.timeScale = 0f;
            playerMovementScript.enabled = false;
            cameraControllerScript.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}