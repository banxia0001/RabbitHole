using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public bool canMove = false;
  
    public float angleRotationNow = 0;
    public float angleRotationShould = 0;
    public float angleTurningModi = 1;

    public bool wandering = false;

    public float modi = 1;

    public GameObject Girl = null;
    public GameObject Rabit = null;
    public float speedModi;
    public float baseSpeed;
    public float isBack;
    public float[] ModiList;
    public float[] HarderModiList;
    public bool HardMode;

    private void Awake()
    {
        Girl = GameObject.FindGameObjectWithTag("Player");
        if (HardMode)
        {
            modi = HarderModiList[Random.Range(1, HarderModiList.Length)];
        }
        else
        {
            modi = ModiList[Random.Range(1, ModiList.Length)];
        }
    }
    public void ResetModi()
    {
        modi = HarderModiList[Random.Range(1, HarderModiList.Length)];
    }
    void Start()
    {
       
        findPlayerPos(5);
        canMove = false;
        gotoPos = ArrowPoint.transform.position;
    }

    public LineRenderer lineRenderer;
    public GameObject Arrow = null;
    public GameObject ArrowPoint = null,LookOnceDir,LookConstantDir;
    public Vector3 gotoPos;
    public float distanceOffset = 0;

    public void MakeArrowAngle()
    {
        Arrow.transform.rotation = Quaternion.AngleAxis(angleRotationNow * modi, Vector3.forward);
        //Arrow.transform.rotation = transform.rotation;
    }

    public void MakeArrowDistance()
    {
        //float distance = Vector3.Distance(this.transform.position, Girl.transform.position);
        //if (distance > 10) distance = 10;
        float distance = 10;
        distanceOffset = 2 + distance / .8f;

        ArrowPoint.transform.localPosition = new Vector3(5 + distanceOffset/10, 0, 0);
        Vector3[] points = new Vector3[2];

        points[0] = transform.position;
        points[1] = ArrowPoint.transform.position;

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
       
    }


    public void SetDestination(float time)
    {
        if (!Stun)
        {
            canMove = true;
            anim.SetTrigger("run");


            //gotoPos = ArrowPoint.transform.position;
            //lineRenderer.gameObject.SetActive(false);
            
        }
        else
        {
            Stun = !Stun;
        }
    }
    public void StopMoving()
    {
        lineRenderer.gameObject.SetActive(false);
        rb.velocity = Vector3.zero;
        canMove = false;
        if (Vector3.Distance(transform.position, Girl.transform.position) > 30)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().SpawnRab();
            Destroy(gameObject);
        }
    }
    public void EnterTimeStop()
    {
        if (!Stun)
        {
            lineRenderer.gameObject.SetActive(true);
            
            AngleChangedSum = 0;
            AngleChangedTemp = 0;
            LookPlayer();
        }
        rb.velocity = Vector3.zero;
        canMove = false;
        
        //if (Random.Range(0, 3) < 2)
        //{
        //    findPlayerPos(19);
        //}
    }
    float AngleChangedTemp,AngleChangedSum;
    bool IsOver180;
    void Update()
    {
        //if (angleRotationNow < angleRotationShould) angleRotationNow += angleTurningModi * Time.deltaTime;
        //if (angleRotationNow > angleRotationShould) angleRotationNow -= angleTurningModi * Time.deltaTime;
        //transform.rotation = Quaternion.AngleAxis(angleRotationNow * modi, Vector3.forward);
        Rabit.transform.rotation = Quaternion.Euler(0, 0, 0);
        LookConstantDir.transform.right= Girl.transform.position - transform.position;
        //angleRotationNow = LookConstantDir.transform.rotation.z - LookOnceDir.transform.rotation.z;
        //Arrow.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angleRotationNow * modi));
        //Arrow.transform.position = Vector3.ProjectOnPlane(Arrow.transform.position, transform.forward);
        ArrowPoint.transform.localPosition = new Vector3(5 + 8 / 10, 0, 0);
        //float AngleChanged = Quaternion.Angle(Rabit.transform.rotation, LookConstantDir.transform.rotation);
        float AngleChanged = Rabit.transform.rotation.eulerAngles.z - LookConstantDir.transform.rotation.eulerAngles.z;
        AngleChangedSum += AngleChanged - AngleChangedTemp;
        AngleChangedTemp = AngleChanged;
        
        transform.rotation = LookDir * Quaternion.Euler(Vector3.forward * AngleChangedSum*modi);
        Vector3[] points = new Vector3[2];

        points[0] = transform.position;
        points[1] = ArrowPoint.transform.position;

        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
        
        //MakeArrowAngle();
        //MakeArrowDistance();

        //if (transform.position != gotoPos)
        //{
        //    transform.position = Vector3.Lerp(transform.position, gotoPos, 0.3f);
        //}
        //rb.velocity = 10 * transform.right;
        isBack -= Time.deltaTime;
        if (canMove && isBack < 0)
        {
            float distance = Vector3.Distance(this.transform.position, gotoPos);
            float thisSpeed = speedModi * distance + baseSpeed;
            //Debug.Log(thisSpeed);
            gotoPos = transform.TransformPoint( Vector3.right* 2);
            Vector2 tempTarget = new Vector2(-gotoPos.x, gotoPos.y);
            rb.AddForce(transform.right/3,ForceMode2D.Impulse);
        }
    }

    private void findPlayerPos(float value)
    {
        Vector3 dir = Girl.transform.position - transform.position;
        //if (wandering)
        //{
        //    value += 300;
        //    angleRotationShould = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //}
        
        angleRotationShould = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    Quaternion LookDir;
    void LookPlayer()
    {
        Vector3 dir = Girl.transform.position - transform.position;
        //LookOnceDir.transform.right = dir;
        transform.right = dir;
        LookDir = transform.rotation;
        //Arrow.transform.right = dir;
        //Debug.Log("Looked");
    }
    public bool Stun;
    public int StunedTimes=0;
    private void OnCollisionEnter2D(Collision2D other)
    {
        
    //}
    //void OnTriggerEnter2D(Collider2D other)
    //{
        if (other.gameObject.CompareTag("Wall"))
        {
            //stun once when hit wall
            Stun = true;
            rb.velocity = Vector3.zero;
            StunedTimes++;
            if (StunedTimes > 3)
            {
                Destroy(gameObject);
            }
            //findPlayerPos(5);
        }
        if (other.gameObject.CompareTag("Rabit"))
        {
            //stun once when hit wall
            Stun = true;
            rb.velocity = Vector3.zero;
            StunedTimes++;
            if (StunedTimes > 3)
            {
                Destroy(gameObject);
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {
            //hitback
            isBack = .1f;
            Vector2 newVector = gameObject.transform.position - other.gameObject.transform.position;
            rb.AddForce(newVector*3 ,ForceMode2D.Impulse);

            other.gameObject.GetComponent<Girl>().hitBy();
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-newVector*2, ForceMode2D.Impulse);
            //findPlayerPos(5);
            LookPlayer();
        }

    }


 
}
