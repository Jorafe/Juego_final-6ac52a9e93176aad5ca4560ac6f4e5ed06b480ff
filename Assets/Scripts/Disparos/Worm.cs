using UnityEngine;
using System.Collections;
using TMPro;

public class Worm : MonoBehaviour
{
    public enum EnemyType { Worm, Spider }
    public EnemyType enemyType = EnemyType.Worm;

    [Header("Canvas flotante con el texto de caramelos ganados")]
    public GameObject candyCanvas; // Canvas con el texto
    public TextMeshProUGUI candyText; // Texto del canvas

    private void OnTriggerEnter(Collider other)
    {
        // Verifica si la colisión es con una bala (comprobando capa)
        if (other.gameObject.layer == LayerMask.NameToLayer("WhatIsBullet"))
        {
            StartCoroutine(DeactivateCoroutine());
        }
    }

    private IEnumerator DeactivateCoroutine()
    {
        // Sumar caramelos según el tipo de enemigo
        int gainedCandy = 0;
        switch (enemyType)
        {
            case EnemyType.Worm:
                gainedCandy = 1;  // Gusano otorga 1 caramelo
                break;
            case EnemyType.Spider:
                gainedCandy = 2;  // Araña otorga 2 caramelos
                break;
        }

        // Añadir caramelos a la puntuación global
        CandyShoot.Instance.AddCandies(gainedCandy);

        // Mostrar canvas flotante si está asignado
        if (candyCanvas != null && candyText != null)
        {
            candyText.text = "0";  // Comienza desde 0
            candyCanvas.SetActive(true);
            // Pasamos la corutina a CandyShoot para manejar la animación
            CandyShoot.Instance.StartCoroutine(CandyShoot.Instance.AnimateCandyText(gainedCandy, candyText));
        }

        // Desactivar el enemigo de inmediato
        gameObject.SetActive(false);

        // Esperar 1 segundo antes de desactivar el canvas flotante
        yield return new WaitForSeconds(1f);
        if (candyCanvas != null)
        {
            candyCanvas.SetActive(false);  // Desactivar el canvas después de 1 segundo
        }
    }
}