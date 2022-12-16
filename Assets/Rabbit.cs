using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator anim;
    public bool canMove = false;
  
   

    public bool wandering = false;

    public float modi = 1;

    public GameObject Girl = null;
    public GameObject Rabit = null;
    public float baseSpeed;
    public float isBack;
    public float[] ModiList;

    private void Awake()
    {
        Girl = GameObject.FindGameObjectWithTag("Player");
        

        rabBirth();
    }
    void Start()
    {
       
        canMove = false;
        gotoPos = ArrowPoint.transform.position;
        LookPlayer();
    }

    public LineRenderer lineRenderer;
    public LineRenderer lineRenderer2;
    public GameObject Arrow;
    public GameObject ArrowPoint = null,LookOnceDir,LookConstantDir;
    public Vector3 gotoPos;
    public float distanceOffset = 0;

    public void SetDestination(float time)
    {
        if (!Stun)
        {
            canMove = true;
            anim.SetTrigger("run");
            lineRenderer.gameObject.SetActive(true);
            lineRenderer2.gameObject.SetActive(false);
        }
        else
        {
            Stun = !Stun;
        }
    }

    public GameObject birthEffect;
    public GameObject deathEffect;
    private bool isDying;
    public void rabDeath()
    {
        if (isDying) return;
        isDying = true;
        GameObject go = Instantiate(deathEffect, transform.position, Quaternion.identity, this.gameObject.transform.parent) as GameObject;
        StartCoroutine(death());
    }

    public IEnumerator death()
    {
        rb.velocity = Vector3.zero;
        canMove = false;
        anim.SetTrigger("death");
        yield return new WaitForSeconds(.4f);
        Destroy(this.gameObject);
    }
    public void rabBirth()
    {
        GameObject go = Instantiate(birthEffect, transform.position, Quaternion.identity, this.gameObject.transform.parent) as GameObject;
    }
    public void StopMoving()
    {
        lineRenderer.gameObject.SetActive(false);
        lineRenderer2.gameObject.SetActive(true);
        rb.velocity = Vector3.zero;
        canMove = false;
    }
    public void EnterTimeStop()
    {
        lineRenderer.gameObject.SetActive(false);
        lineRenderer2.gameObject.SetActive(true);

        if (!Stun)
        {
            LookPlayer();
        }
        rb.velocity = Vector3.zero;
        canMove = false;
    }


    public float angleRotation = 0;
    private Vector2 faceDirection;
    public float angleTurningModi = 1;
    void LookPlayer()
    {
        faceDirection = (Girl.transform.position - transform.position).normalized;
        angleRotation = Mathf.Atan2(faceDirection.y, faceDirection.x) * Mathf.Rad2Deg;
    }
    private void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, angleRotation * modi);
        Rabit.transform.rotation = Quaternion.Euler(0, 0, 0);

        Vector3[] points = new Vector3[2];
        points[0] = transform.position;
        points[1] = ArrowPoint.transform.position;
        lineRenderer.positionCount = points.Length;
        lineRenderer2.positionCount = points.Length;

        lineRenderer.SetPositions(points);
        lineRenderer2.SetPositions(points);
    }

    public void SetValue(float speedModi)
    {
        baseSpeed += speedModi;
        modi = ModiList[Random.Range(0, ModiList.Length)];
    }
    void FixedUpdate()
    {
        isBack -= Time.deltaTime;
        if (canMove && isBack < 0)
        {
            if (isDying) return;
            //float speed = 3 - Vector3.Distance(this.transform.position, gotoPos);
         
            //if (speed < 0) speed = .5f;

            //Debug.Log(thisSpeed);
            gotoPos = transform.TransformPoint(Vector3.right * 2);
            Vector2 tempTarget = new Vector2(-gotoPos.x, gotoPos.y);
            rb.AddForce(transform.right / 1.2f * baseSpeed, ForceMode2D.Impulse);
        }

    }



    Quaternion LookDir;
    
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
                rabDeath();
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
                rabDeath();
            }
        }

        if (other.gameObject.CompareTag("Player"))
        {

            //hitback
            isBack = .2f;
            Vector2 newVector = gameObject.transform.position - other.gameObject.transform.position;
            rb.AddForce(newVector* 12 ,ForceMode2D.Impulse);



            other.gameObject.GetComponent<Girl>().hitBy(canMove);
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(-newVector* 12, ForceMode2D.Impulse);
            //findPlayerPos(5);
            LookPlayer();
        }

    }


 
}
