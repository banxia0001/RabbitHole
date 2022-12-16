using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private bool enter = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (enter) return;


            enter = true;

            int num = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().sceneNum;
            if (num == 3)
            {
                Controller.LoadScene(0);
                return;
            }
            Controller.LoadScene(num + 1);
        }
    }

}