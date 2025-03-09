using UnityEngine;
using TMPro;  // Asegúrate de que tienes este namespace si usas TextMeshPro
using UnityEngine.UI;  // Necesario para el uso de Text de Unity UI
using System.Collections; // Necesario para las corutinas

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

    void Start()
    {
        timeManager = FindObjectOfType<TimerManager>();
        candyManager = FindObjectOfType<CandyManager>();  // Encuentra el CandyManager en la escena
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

        // Desactiva el menú de pausa en el TimerManager (si está activo)
        if (timeManager != null && timeManager.pauseMenu != null)
        {
            timeManager.pauseMenu.SetActive(false);
        }

        // Desactiva el GameObject del jugador (evita que el jugador se mueva)
        if (player != null)
        {
            player.SetActive(false); // Desactiva el GameObject del jugador
        }

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
}