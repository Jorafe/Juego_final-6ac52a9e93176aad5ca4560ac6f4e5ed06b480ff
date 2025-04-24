using UnityEngine;

public class Worm : MonoBehaviour
{
    public enum EnemyType { Worm, Spider }
    public EnemyType enemyType = EnemyType.Worm;

    private bool isDeactivated = false; // Aseguramos que no se cuente más de una vez

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WhatIsBullet") && !isDeactivated)
        {
            isDeactivated = true;  // Marca como desactivado este enemigo
            Deactivate();  // Procede a desactivarlo
        }
    }

    private void Deactivate()
    {
        int gainedCandy = (enemyType == EnemyType.Spider) ? 2 : 1;
        
        // Añadir caramelos al contador global
        LevelManager.Instance.AddCandies(gainedCandy);

        // Llamar a la función para desactivar el enemigo en LevelManager
        //LevelManager.Instance.RegisterEnemyDeactivated();  // Solo se cuenta una vez

        // Desactivar este enemigo
        gameObject.SetActive(false);
    }
}