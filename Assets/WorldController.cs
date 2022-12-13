using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldController : MonoBehaviour
{
    public GameObject WorldFolder;
    public GameObject Girl;

    public float beatTime_;
    public float beatTime;
    public float beatTimeModi;

    public bool isStopingTime = false;
    public Animator camAnim;
    public Animator beatAnim;


    void Awake()
    {
        beatTime = beatTimeModi;
        camAnim.SetTrigger("start");
        isStopingTime = false;
        beatAnim.speed = 1/ beatTime_;
        //if (Controller.start == false)
        //{
        //    camAnim.SetTrigger("start");
        //    Controller.start = true;
        //}
    }

    private void Start()
    {
        EnterStop();

    }

    public float dirModiL = 1;
    public float dirModiR = 1;
    void Update()
    {
        float dir = 0;

        if (Input.GetAxis("Mouse X") < 0)
        {
            dir = (1.6f + dirModiL) * -.22f;
            dirModiL += 5 * Time.deltaTime;
        }
        else if (Input.GetAxis("Mouse X") > 0)
        {
            dir = (1.6f + dirModiR) * .22f;
            dirModiR += 5 * Time.deltaTime;
        }

        else
        {
            dirModiL = 1;
            dirModiR = 1;
        } 

        WorldFolder.transform.RotateAround(Girl.transform.position, Vector3.forward,160 * dir * Time.deltaTime);
        
        
        beatTime -= Time.deltaTime;

        if (beatTime < 0)
        {
            if (isStopingTime)
            {
                isStopingTime = false;
                LeaveStop();
            }

            else
            {
                isStopingTime = true;
                EnterStop();
            }
            beatTime = beatTime_;
        }

    }

    public void EnterStop()
    {
        Girl.GetComponent<Girl>().EnterTimeStop();
        beatAnim.SetTrigger("run");

        GameObject[] rabits = GameObject.FindGameObjectsWithTag("Rabit");
        foreach (GameObject rabit in rabits)
        {
            Rabbit rab = rabit.GetComponent<Rabbit>();
            rab.EnterTimeStop();
        }
    }

    public void LeaveStop()
    {
        Girl.GetComponent<Girl>().LeaveTimeStop();
        camAnim.SetTrigger("run");
        GameObject[] rabits = GameObject.FindGameObjectsWithTag("Rabit");
        foreach (GameObject rabit in rabits)
        {
            Rabbit rab = rabit.GetComponent<Rabbit>();
            rab.SetDestination(beatTime_);
        }
    }

}