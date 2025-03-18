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

    private int bulletsFired = 0;
    private int wormsDeactivated = 0;
    private int maxBulletsPerPhase = 25;
    private int totalWorms = 60;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        victoryMenu.SetActive(false);  // Aseguramos que el menú de victoria esté oculto al principio
    }

    public void RegisterShot()
    {
        bulletsFired++;
        UpdateUI();

        if (bulletsFired >= maxBulletsPerPhase || wormsDeactivated >= totalWorms)
        {
            StartCoroutine(ShowVictoryMenu());  // Llamamos a la corutina para esperar 1 segundo
        }
    }

    public void RegisterWormDeactivated()
    {
        wormsDeactivated++;
        UpdateUI();

        if (wormsDeactivated >= totalWorms)
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
            wormCounterText.text = wormsDeactivated + "/" + totalWorms;
        }

        if (ammoCounterText != null)
        {
            ammoCounterText.text = (maxBulletsPerPhase - bulletsFired) + "/" + maxBulletsPerPhase;
        }
    }
}