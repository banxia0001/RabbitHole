using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class WorldController : MonoBehaviour
{
    public GameObject WorldFolder;
    public GameObject Girl;

    public int sceneNum;
    public float diffModi = 0;
    public int rabitnum = 0;
    public int rabitnumMax = 20;
    


    public float beatTime_;
    public float beatTime;
    public float beatTimeModi;

    public bool isStopingTime = false;
    public bool iStopingPlayer = false;
    public Animator camAnim;
    public Animator beatAnim;

    public GameObject Rock;
    public Transform MapFolder, MonsterFolder;
    void Awake()
    {
        beatTime = beatTimeModi;
        camAnim.SetTrigger("start");
        isStopingTime = true;
        iStopingPlayer = true;
        beatAnim.speed = 1 / beatTime_;
        //for(int i = -50; i < 100; i++)
        //{
        //    for (int o = -50; o < 100; o++)
        //    {
        //        Vector3 tPos = new Vector3(i * 2 + Random.Range(-3.0f, 3.0f), o * 2 + Random.Range(-3.0f, 3.0f), 0);
        //        Instantiate(Rock, tPos, Quaternion.identity, MapFolder);
        //    }
        //}
        Girl.GetComponent<Girl>().ClearRocksInRadius();
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
        if (isStopingTime)
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

            WorldFolder.transform.RotateAround(Girl.transform.position, Vector3.forward, 160 * dir * Time.deltaTime);
        }
        beatTime -= Time.deltaTime;

        if (beatTime < 0)
        {

            if (iStopingPlayer)
            {
                iStopingPlayer = false;
                //rab stop, player start
                GirlLeaveStop();
                RabEnterStop();
                spawnRab();
                //for (int i = 0; i < Random.Range(1, 4); i++)
                //{
                //    SpawnRab();
                //}
            }
            else
            {
                iStopingPlayer = true;
                isStopingTime = true;
                //both stop
                GirlEnterStop();
                RabLeaveStop();

                beatTime = beatTime_;
                return;
            }

            beatTime = beatTime_;
        }

    }
    public GameObject Rab;
    public int SpawnedNum = 0;


    public GameObject GuidePoint;
    public GameObject GuidePoint2;

 
    public void spawnRab()
    {
        GameObject[] spawnList = GameObject.FindGameObjectsWithTag("SpawnPos");

        List<GameObject> sortList2 = new List<GameObject>();
        for (int i3 = 0; i3 < spawnList.Length; i3++)
        {
            sortList2.Add(spawnList[i3]);
        }

        Vector3 tPos = GuidePoint.transform.position + new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0);

        List<GameObject> sortList = new List<GameObject>();

        for (int i2 = 0; i2 < Random.Range(2 + rabitnum, 4 + rabitnum); i2++)
        {
            GameObject nearestPos = sortList2[0];
            float nearValue = 99999;

            for (int i = 0; i < sortList2.Count; i++)
            {

                float dist = Vector3.Distance(tPos, sortList2[i].transform.position);

                if (dist < nearValue)
                {
                    nearValue = dist;
                    nearestPos = sortList2[i];
                }
            }
            sortList2.Remove(nearestPos);
            sortList.Add(nearestPos);
        }



        foreach (GameObject spawn in sortList)
        {
            GameObject rab = Instantiate(Rab, spawn.transform.position, Quaternion.identity, MonsterFolder);


            rab.GetComponent<Rabbit>().SetValue(diffModi);
            rab.GetComponent<Rabbit>().EnterTimeStop();
    
        }

        if (GameObject.FindGameObjectsWithTag("Rabit") != null)
        {
            GameObject[] myRab = GameObject.FindGameObjectsWithTag("Rabit");

            Debug.Log("long" + myRab.Length);
            if (myRab.Length > rabitnumMax)
            {
               
                delRab(myRab.Length - rabitnumMax);
            }
        }
      

    }
    public void delRab(int maxDelNum)
    {
        GameObject[] myRab = GameObject.FindGameObjectsWithTag("Rabit");
        List<GameObject> sortList2 = new List<GameObject>();
        for (int i3 = 0; i3 < myRab.Length; i3++)
        {
            sortList2.Add(myRab[i3]);
        }

        Vector3 tPos = GuidePoint2.transform.position + new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), 0);
        List<GameObject> rabList = new List<GameObject>();

        for (int i2 = 0; i2 < Random.Range(2, 4); i2++)
        {
            GameObject nearestRab = sortList2[0];
            float nearValue = 99999;

            for (int i = 0; i < sortList2.Count; i++)
            {
                float dist = Vector3.Distance(tPos, sortList2[i].transform.position);

                if (dist < nearValue)
                {
                    nearValue = dist;
                    nearestRab = sortList2[i];
                }
            }
            sortList2.Remove(nearestRab);
            rabList.Add(nearestRab);
        }

        

        
        for (int i2 = 0; i2 < maxDelNum -1 ; i2++)
        {
            if (rabList.Count <= i2) 
                break;
            rabList[i2].GetComponent<Rabbit>().rabDeath();
        }
    }

    //public void DelFarestRab()
    //{ 




    //}
    public void SpawnRab()
    {

        //Vector3 tPos = Vector3.ProjectOnPlane(Vector3.right * Random.Range(10.0f, 15.0f), new Vector3(Random.Range(0.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0));
       //Vector3 tPos = new Vector3(Random.Range(20.0f, 30.0f), Random.Range(-25.0f, 25.0f), 0);
        //GameObject tempRab = Instantiate(Rab, Girl.transform.TransformPoint( tPos), Quaternion.identity, MonsterFolder);
        //tempRab.GetComponent<Rabbit>().EnterTimeStop();
        //SpawnedNum++;
        //if (SpawnedNum > 15)
        //{
        //    tempRab.GetComponent<Rabbit>().ResetModi();
        //}
    }
    public void EnterStop()
    {
        GirlEnterStop();
        RabEnterStop(); 
    }
    public void GirlEnterStop()
    {
        Girl.GetComponent<Girl>().EnterTimeStop();
        beatAnim.SetTrigger("run");

    }
    public void RabEnterStop()
    {
        GameObject[] rabits = GameObject.FindGameObjectsWithTag("Rabit");
        foreach (GameObject rabit in rabits)
        {
            Rabbit rab = rabit.GetComponent<Rabbit>();
            rab.EnterTimeStop();
        }
    }
    public void RabStop()
    {
        GameObject[] rabits = GameObject.FindGameObjectsWithTag("Rabit");
        foreach (GameObject rabit in rabits)
        {
            Rabbit rab = rabit.GetComponent<Rabbit>();
            rab.StopMoving();
        }
        //also Spawn 2+ rabbit in 180 degree infront of player 
        //distance about 7 units away
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            SpawnRab();
        }
    }
    public void GirlLeaveStop()
    {
        Girl.GetComponent<Girl>().LeaveTimeStop();
        camAnim.SetTrigger("run");
    }
    public void RabLeaveStop()
    {
        GameObject[] rabits = GameObject.FindGameObjectsWithTag("Rabit");
        foreach (GameObject rabit in rabits)
        {
            Rabbit rab = rabit.GetComponent<Rabbit>();
            rab.SetDestination(beatTime_);
        }
    }

}