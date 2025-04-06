using UnityEngine;

public class PlayerCollisionReporter : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        SoundManagerMenu.Instance?.CheckCollision(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        SoundManagerMenu.Instance?.CheckCollision(other.gameObject);
    }
}