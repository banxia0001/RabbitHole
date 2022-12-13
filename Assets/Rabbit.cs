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



    private void Awake()
    {
        Girl = GameObject.FindGameObjectWithTag("Player");
    }
    void Start()
    {
       
        findPlayerPos(5);
        canMove = false;
        gotoPos = ArrowPoint.transform.position;
    }

    public LineRenderer lineRenderer;
    public GameObject Arrow = null;
    public GameObject ArrowPoint = null;
    public Vector3 gotoPos;
    public float distanceOffset = 0;

    public void MakeArrowAngle()
    {
        Arrow.transform.rotation = Quaternion.AngleAxis(angleRotationNow * modi, Vector3.forward);
    }

    public void MakeArrowDistance()
    {
        float distance = Vector3.Distance(this.transform.position, Girl.transform.position);
        if (distance > 10) distance = 10;
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
        anim.SetTrigger("run");

     
      gotoPos = ArrowPoint.transform.position;
        //lineRenderer.gameObject.SetActive(false);
        canMove = true;
    }

    public void EnterTimeStop()
    {
        lineRenderer.gameObject.SetActive(true);
        rb.velocity = Vector3.zero;
        canMove = false;
        if (Random.Range(0, 3) < 2)
        {
            findPlayerPos(19);
        }
    }
    void Update()
    {
        if (angleRotationNow < angleRotationShould) angleRotationNow += angleTurningModi * Time.deltaTime;
        if (angleRotationNow > angleRotationShould) angleRotationNow -= angleTurningModi * Time.deltaTime;
        transform.rotation = Quaternion.AngleAxis(angleRotationNow * modi, Vector3.forward);
        Rabit.transform.rotation = Quaternion.Euler(0, 0, 0);

        MakeArrowAngle();
        MakeArrowDistance();

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
            rb.AddForce(thisSpeed * transform.right);
        }
    }

    private void findPlayerPos(float value)
    {
        Vector3 dir = Girl.transform.position - transform.position;
        if (wandering) value += 300;
        angleRotationShould = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + Random.Range(-value, value);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            findPlayerPos(5);
        }

        if (other.gameObject.CompareTag("Player"))
        {
            isBack = .1f;
            Vector2 newVector = this.gameObject.transform.position - other.gameObject.transform.position;
            rb.AddForce(newVector * 200f);
           


            other.gameObject.GetComponent<Girl>().hitBy();
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-newVector * 200f);
            findPlayerPos(5);
        }

    }


 
}
