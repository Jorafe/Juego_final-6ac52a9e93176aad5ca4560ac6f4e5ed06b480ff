using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float respawnDelay = 1f;  // El tiempo de retraso antes de reaparecer

    private void OnDestroy()
    {
        // Si el objeto del jugador es destruido, espera un retraso para volver a aparecer
        if (gameObject.activeInHierarchy)
        {
            Invoke("Respawn", respawnDelay);
        }
    }

    private void Respawn()
    {
        Transform lastCheckpoint = Checkpoint.GetLastCheckpoint();

        if (lastCheckpoint != null)
        {
            // Respawnea el jugador en el último checkpoint
            transform.position = lastCheckpoint.position;
            transform.rotation = lastCheckpoint.rotation;
            
            // Reactivar el jugador si estaba desactivado
            gameObject.SetActive(true);
        }
    }
}
