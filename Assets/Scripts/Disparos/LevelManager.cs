using UnityEngine;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject player;
    public GameObject victoryMenu;  // Referencia al menú de victoria
    public TMP_Text wormCounterText;
    public TMP_Text ammoCounterText;
    public TMP_Text timeCounterText;  // Contador de tiempo

    private int bulletsFired = 0;
    private int wormsDeactivated = 0;
    private int maxBulletsPerPhase = 25;

    // Nuevo array para los gusanos
    public GameObject[] worms;  // Array de todos los gusanos en el nivel

    private float timeElapsed = 0f;  // Tiempo transcurrido en segundos

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        victoryMenu.SetActive(false);  // Aseguramos que el menú de victoria esté oculto al principio
    }

    private void Update()
    {
        // Actualizamos el tiempo transcurrido
        timeElapsed += Time.deltaTime;
        UpdateTimeUI();
    }

    public void RegisterShot()
    {
        bulletsFired++;
        UpdateUI();

        if (bulletsFired >= maxBulletsPerPhase || wormsDeactivated >= worms.Length)
        {
            StartCoroutine(ShowVictoryMenu());  // Llamamos a la corutina para esperar 1 segundo
        }
    }

    public void RegisterWormDeactivated(GameObject worm)
    {
        // Verifica si el gusano ya ha sido desactivado
        if (!worm.activeInHierarchy) return;

        worm.SetActive(false);  // Desactiva el gusano
        wormsDeactivated++;  // Incrementamos el contador
        UpdateUI();  // Actualizamos la UI inmediatamente

        if (wormsDeactivated >= worms.Length)
        {
            StartCoroutine(ShowVictoryMenu());  // Llamamos a la corutina para esperar 1 segundo
        }
    }

    private IEnumerator ShowVictoryMenu()
    {
        // Esperamos 1 segundo antes de continuar
        yield return new WaitForSeconds(1f);

        // Desactivamos al jugador
        if (player != null)
        {
            player.SetActive(false);
        }

        // Activamos el cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Mostramos el menú de victoria
        victoryMenu.SetActive(true);
    }

    private void UpdateUI()
    {
        // Actualizamos los contadores de gusanos y balas
        if (wormCounterText != null)
        {
            wormCounterText.text = wormsDeactivated + "/" + worms.Length;
        }

        if (ammoCounterText != null)
        {
            ammoCounterText.text = (maxBulletsPerPhase - bulletsFired) + "/" + maxBulletsPerPhase;
        }
    }

    private void UpdateTimeUI()
    {
        // Convertimos el tiempo transcurrido a minutos y segundos
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);
        
        // Actualizamos el texto del contador de tiempo
        if (timeCounterText != null)
        {
            timeCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}