using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller : MonoBehaviour
{
    public static bool start = false;

    public static int life;
    public static void LoadScene(int sceneNum)
    {
        
        SceneManager.LoadScene(sceneNum);
    }

}
