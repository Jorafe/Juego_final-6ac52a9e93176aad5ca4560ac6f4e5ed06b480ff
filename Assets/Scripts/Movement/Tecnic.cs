using UnityEngine;

public class Tecnic : MonoBehaviour
{
    private TimerManager timeManager;

    void Start()
    {
        timeManager = FindObjectOfType<TimerManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("WhatIsTimer"))
        {
            if (timeManager != null)
            {
                timeManager.StopTimer();
            }
        }
    }
}
