using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStart : MonoBehaviour
{

   public float value1 = 0;
    public float value2 = 0;
    public Animator anim;
    bool enter = false;
    bool enterTuto = false;
    public AudioSource scream;

    public GameObject G1,G2;


    private void Start()
    {
        Controller.life = 10;
    }
    void FixedUpdate()
    {

        if (enter) return;
        if (Input.GetAxis("Mouse X") < 0)
        {
            value1++;
        }
       if (Input.GetAxis("Mouse X") > 0)
        {
            value2++;
        }

        if (value1 > 40)
        {
            tuto();
        }

        if (value2 > 40)
        {
            if (enterTuto)
            {
                tutoBack();

            }

            else
            {
                StartCoroutine(toB());
            }
        
        }

    }

    public void tuto()
    {
        G2.SetActive(true);
        G1.SetActive(false);

        value1 = 0;
        value2 = 0;

        enterTuto = true;
    }

    public void tutoBack()
    {
        G2.SetActive(false);
        G1.SetActive(true);

        value1 = 0;
        value2 = 0;

        enterTuto = false;
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
