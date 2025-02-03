using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : MonoBehaviour
{
    private PlayerRespawn _playerRespawn;

    void Awake()
    {
        _playerRespawn = GameObject.FindObjectOfType<PlayerRespawn>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerRespawn.transform.position = _playerRespawn.respawnPosition;
        }
    }

}
