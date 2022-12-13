using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public static bool start = false;
    public static int sceneNumber;


    public static void LoadScene()
    {
        Controller.sceneNumber++;
        SceneManager.LoadScene(Controller.sceneNumber);
    }

    public static void LoadSceneDeath()
    {
        SceneManager.LoadScene(0);
    }
}
