using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPosition;
    public float respawnDelay = 1f;  // El tiempo de retraso antes de reaparecer

    // Actualización por fotograma
    void Update()
    {
        // Detecta si se presiona la tecla 'L'
        if (Input.GetKeyDown(KeyCode.L))
        {
            // Respawnea inmediatamente al jugador
            StartCoroutine(RespawnPlayer());
        }
    }

    // Función para respawnear al jugador con retraso (puedes quitar el retraso si prefieres que sea inmediato)
    IEnumerator RespawnPlayer()
    {
        yield return new WaitForSeconds(respawnDelay);  // Si no quieres retraso, elimina esta línea.
        transform.position = respawnPosition;  // Reubica al jugador en la posición de respawn
    }
}