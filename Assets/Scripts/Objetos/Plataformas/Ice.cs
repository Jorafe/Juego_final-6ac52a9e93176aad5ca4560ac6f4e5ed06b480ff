using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float iceFriction = 0.98f; // Factor de deslizamiento (0.98 = se desliza lentamente, 1 = sin fricción)

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionStay(Collision collision)
    {
        // Si el objeto tocado tiene la tag "WhatIsIce"
        if (collision.gameObject.CompareTag("WhatIsIce"))
        {
            // Mantiene la velocidad horizontal reduciendo la fricción gradualmente
            rb.velocity = new Vector3(rb.velocity.x * iceFriction, rb.velocity.y, rb.velocity.z * iceFriction);
        }
    }
}
