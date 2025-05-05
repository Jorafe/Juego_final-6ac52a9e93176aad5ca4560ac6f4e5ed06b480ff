using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 respawnPosition;
    public float respawnDelay = 1f;  // El tiempo de retraso antes de reaparecer
    private Rigidbody _rBody;
    private CarController carcontroler;

    void Awake()
    {
        _rBody = GetComponent<Rigidbody>();
        carcontroler = GetComponent<CarController>();
    }

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
        _rBody.velocity = Vector3.zero;
        _rBody.isKinematic = true;
        carcontroler.enabled = false;
        transform.position = respawnPosition;  // Reubica al jugador en la posición de respawn
        transform.rotation = Quaternion.Euler(0, 180, 0);
        //yield return new WaitForSeconds(0.1);
        _rBody.isKinematic = false;
        carcontroler.enabled = true;

    }
}