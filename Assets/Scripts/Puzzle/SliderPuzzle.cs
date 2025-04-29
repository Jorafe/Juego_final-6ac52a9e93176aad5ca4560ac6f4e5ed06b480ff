using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPuzzle : MonoBehaviour
{
    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform. Translate(0, 0, 1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform. Translate(0, 0, -1);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform. Translate(1, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform. Translate(-1, 0, 1);
        }
    }
}
