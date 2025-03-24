using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static Transform lastCheckpoint = null;  // Guarda el último checkpoint tocado
    private static GameObject lastCheckpointObject = null;  // Guarda el objeto del último checkpoint tocado

    [SerializeField] private PlayerRespawn respawn;

    void Awake()
    {
        respawn = FindObjectOfType<PlayerRespawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            respawn.respawnPosition = transform.position;

            Destroy(gameObject);
        }
    }

    public static Transform GetLastCheckpoint()
    {
        return lastCheckpoint;
    }
}
