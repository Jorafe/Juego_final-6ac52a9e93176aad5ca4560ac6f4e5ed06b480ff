using UnityEngine;
using TMPro;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject player;
    public GameObject victoryMenu;
    public TMP_Text wormCounterText;
    public TMP_Text ammoCounterText;
    public TMP_Text timeCounterText;
    public TMP_Text candyCounterText;

    private int bulletsFired = 0;
    private int enemiesDeactivated = 0; // Evitar incrementar dos veces
    private int totalCandies = 0;
    private int maxBulletsPerPhase = 20;

    public GameObject[] enemies;
    private float timeElapsed = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
        victoryMenu.SetActive(false);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        UpdateTimeUI();
    }

    public void RegisterShot()
    {
        bulletsFired++;
        UpdateUI();

        if (bulletsFired >= maxBulletsPerPhase || enemiesDeactivated >= enemies.Length)
        {
            StartCoroutine(ShowVictoryMenu());
        }
    }

    public void RegisterEnemyDeactivated()
    {
        // Solo incrementamos el contador si no hemos llegado al total de enemigos
        if (enemiesDeactivated < enemies.Length)
        {
            enemiesDeactivated++;  // Incrementar solo una vez
            UpdateUI();  // Actualizar UI para reflejar el nuevo contador de enemigos
        }

        if (enemiesDeactivated >= enemies.Length)
        {
            StartCoroutine(ShowVictoryMenu());
        }
    }

    public void AddCandies(int amount)
    {
        totalCandies += amount;
        UpdateUI();  // Actualizar UI para reflejar los caramelos ganados
    }

    private IEnumerator ShowVictoryMenu()
    {
        yield return new WaitForSeconds(1f);

        if (player != null)
        {
            player.SetActive(false);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        victoryMenu.SetActive(true);
    }

    private void UpdateUI()
    {
        if (wormCounterText != null)
        {
            wormCounterText.text = enemiesDeactivated + "/" + enemies.Length;  // Contador de enemigos desactivados
        }

        if (ammoCounterText != null)
        {
            ammoCounterText.text = (maxBulletsPerPhase - bulletsFired) + "/" + maxBulletsPerPhase;
        }

        if (candyCounterText != null)
        {
            candyCounterText.text = totalCandies.ToString(); // Solo el número de caramelos
        }
    }

    private void UpdateTimeUI()
    {
        int minutes = Mathf.FloorToInt(timeElapsed / 60f);
        int seconds = Mathf.FloorToInt(timeElapsed % 60f);

        if (timeCounterText != null)
        {
            timeCounterText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}