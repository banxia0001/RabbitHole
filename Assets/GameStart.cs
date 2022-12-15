using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

    float value = 0;
    public Animator anim;
    bool enter = false;
    public AudioSource scream;
    void Update()
    {
        if (Input.GetAxis("Mouse X") < 0)
        {
            value++;
        }
        else if (Input.GetAxis("Mouse X") > 0)
        {
            value++;
        }

        if (value > 20 && enter == false)
        {
            StartCoroutine(toB());
        }

    }


    public IEnumerator toB()
    {
        scream.Play();
        enter = true;
        anim.SetTrigger("hhaa");
        yield return new WaitForSeconds(.8f);
        Controller.LoadScene(1);
    }
}
