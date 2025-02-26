using UnityEngine;

public class Tecnic : MonoBehaviour
{
    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WhatIsTimer"))
        {
            if (gameManager != null)
            {
                gameManager.StopTimer();
            }
        }
    }
}
