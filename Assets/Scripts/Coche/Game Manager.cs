using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
     void Update()
    {
        // Check if the F1 key is pressed
        if (Input.GetKeyDown(KeyCode.F1))
        {
             SceneManager.LoadScene("Plataformas 1");
        }
         if (Input.GetKeyDown(KeyCode.F2))
        {
             SceneManager.LoadScene("Plataformas 2");
        }
         if (Input.GetKeyDown(KeyCode.F3))
        {
             SceneManager.LoadScene("Plataformas 3");
        }
         if (Input.GetKeyDown(KeyCode.F4))
        {
             SceneManager.LoadScene("Pistas de carreras");
        }
         if (Input.GetKeyDown(KeyCode.F5))
        {
             SceneManager.LoadScene("Disparos");
        }
         if (Input.GetKeyDown(KeyCode.F6))
        {
             SceneManager.LoadScene("TestScene");
        }
    }

}
