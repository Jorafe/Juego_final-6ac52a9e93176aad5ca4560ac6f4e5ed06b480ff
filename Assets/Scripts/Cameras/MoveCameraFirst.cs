using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraFirst : MonoBehaviour
{
    public Transform camaraPosition;

    private void Update()
    {
        transform.position = camaraPosition.position;
    }
}
