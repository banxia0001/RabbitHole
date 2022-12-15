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
            Controller.LoadScene(GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().sceneNum + 1);
        }
    }
}
