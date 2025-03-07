using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject player;
    public List<GameObject> platforms;
    public List<GameObject> worms; // Lista de gusanos
    public TMP_Text uiText; // Texto de balas y gusanos
    
    public TextMeshProUGUI wormCounterText; // Referencia al texto en el Canvas
    public TextMeshProUGUI ammoCounterText; // Referencia al texto de balas en el Canvas

    private int currentPhase = 0;
    private int maxPhases = 3; // Número total de fases
    private int bulletsFired = 0;
    private int maxBulletsPerPhase = 25;
    private int wormsDeactivated = 0; // Acumulación total de gusanos derribados
    private int totalWorms = 60; // Máximo de gusanos en todo el juego

    private void Awake()
    {
        Instance = this; // Instancia de LevelManager
    }

    private void Start()
    {
        UpdateUI();
    }

    // Método que retorna el número total de gusanos
    public int GetTotalWorms()
    {
        return totalWorms;
    }

    public void RegisterShot()
    {
        bulletsFired++;
        UpdateUI();

        // Si se acaban las balas o se han desactivado todos los gusanos, cambiar de fase
        if (bulletsFired >= maxBulletsPerPhase || wormsDeactivated >= totalWorms)
        {
            NextPhase();
        }
    }

    public void RegisterWormDeactivated()
    {
        wormsDeactivated++;
        UpdateUI();

        // Si se han desactivado todos los gusanos de esta fase antes de gastar todas las balas, cambiar de fase
        if (AllWormsDisabled())
        {
            NextPhase();
        }
    }

    private bool AllWormsDisabled()
    {
        foreach (GameObject worm in worms)
        {
            if (worm.activeSelf) return false;
        }
        return true;
    }

    public void NextPhase()
    {
        if (currentPhase < maxPhases - 1)
        {
            currentPhase++;
            bulletsFired = 0; // Reiniciar balas en la nueva fase
            ActivateWorms();
            UpdateUI();
        }
        else
        {
            Debug.Log("Juego terminado");
            // Aquí puedes añadir la lógica para finalizar el juego
        }
    }

    private void ActivateWorms()
    {
        foreach (GameObject worm in worms)
        {
            worm.SetActive(true);
        }
    }

    private void UpdateUI()
    {
        // Actualiza el contador de gusanos de esta fase en el formato "X/60"
        if (wormCounterText != null)
        {
            wormCounterText.text = wormsDeactivated + "/" + totalWorms;
        }

        // Actualiza el contador de balas de forma descendente (balas restantes)
        if (ammoCounterText != null)
        {
            ammoCounterText.text = (maxBulletsPerPhase - bulletsFired) + "/" + maxBulletsPerPhase;
        }
    }
}