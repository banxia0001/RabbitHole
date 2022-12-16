using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Girl : MonoBehaviour
{
    //public WorldController WC;
    public int health = 3;
    public float speed = 10f;
    //public float maxSpeed;
    //public float minSpeed;
    public float accelerate;
    public float isBack = 0;
    public Rigidbody2D rb;
    public bool canMove = true;
    public GameObject myGuide;
    public Animator anim;
    public Animator doorAnim;

    public GameObject[] myHearts;
    public GameObject blood;

    [Header("Collision")]
    private Collider2D coll;                            
    public float collisionRadius = 5f;
    Vector2 beg;                                        
    Vector2 down = new Vector2(0, -1);
    public LayerMask HitLayer;
    public Vector2 leftOffsetUp;
    public Vector2 leftOffset;
    public Vector2 leftOffsetDown;
    public Vector2 rightOffsetUp;
    public Vector2 rightOffset;
    public Vector2 rightOffsetDown;

    public AudioSource hitSound;
    public AudioSource deathSound;
    private void SetHeart()
    {
        for (int i = 0; i < myHearts.Length; i++)
        {
            myHearts[i].SetActive(false);
        }

        for (int i = 0; i < myHearts.Length; i++)
        {
            if (i >= health) break;
            myHearts[i].SetActive(true);
        }

    }
    public void Start()
    {
        health = 3;
        SetHeart();
        rb = gameObject.GetComponent<Rigidbody2D>();
        //speed = minSpeed;
        isBack = 0f;
    }
    public void FixedUpdate()
    {
        Vector3 dir = doorAnim.gameObject.transform.position - myGuide.transform.position;
        float angleRotationShould = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        myGuide.transform.rotation = Quaternion.Euler(0, 0, angleRotationShould);
        isHit -= Time.deltaTime;
        checkHitWall();
        //transform.rotation = Quaternion.Euler(0, 0, 0);

        if (isBack > 0)
        {
            isBack -= Time.fixedDeltaTime;
            return;
        }


        if (!canMove) return;
        if (isDying) return;

        rb.AddForce(speed * transform.right);
     
    }

    public void EnterTimeStop()
    {
        canMove = false;
        rb.velocity = Vector3.zero;
        doorAnim.SetTrigger("run");
    }

    public void LeaveTimeStop()
    {
        canMove = true;
        anim.SetTrigger("run");
    }


    private void checkHitWall()
    {
        if(Physics2D.OverlapCircle((Vector2)transform.position + rightOffsetUp, collisionRadius, HitLayer))
        { HitByWall(rightOffsetUp,1);return; }
        if (Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, HitLayer))
        { HitByWall(rightOffset,1); return; }
        if (Physics2D.OverlapCircle((Vector2)transform.position + rightOffsetDown, collisionRadius, HitLayer))
        { HitByWall(rightOffsetDown,1); return; }

        if (Physics2D.OverlapCircle((Vector2)transform.position + leftOffsetUp, collisionRadius, HitLayer))
        { HitByWall(leftOffsetUp,.25f); return; }
        if (Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, HitLayer))
        { HitByWall(leftOffset,.25f); return; }
        if (Physics2D.OverlapCircle((Vector2)transform.position + leftOffsetDown, collisionRadius, HitLayer))
        { HitByWall(leftOffsetDown,.25f); return; }
    }

    private void HitByWall(Vector2 comeDir, float speedUp)
    {
        rb.velocity = Vector3.zero;
        Vector3 comeDir3D = new Vector3(comeDir.x, comeDir.y, 0);
        Vector3 dir = (comeDir3D - new Vector3(0, 0, 0)).normalized;
        rb.AddForce(-dir * 2.2f, ForceMode2D.Impulse);
        isBack = .15f;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] {  rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffsetUp, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffsetDown, collisionRadius);


        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffsetUp, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffsetDown, collisionRadius);
    }


    public LayerMask RockMask;
    public void ClearRocksInRadius()
    {
        Collider2D[] Objs = Physics2D.OverlapCircleAll(transform.position, 11f, RockMask);
        if (Objs.Length > 0)
        {
            for (int i = 0; i < Objs.Length; i++)
            {
                Destroy(Objs[i].gameObject);
            }
        }
    }
    private float isHit;
    public void hitBy(bool hit)
    {
        if (isHit > 0) return;

        hitSound.Play();
        WorldController WC = GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>();
        WC.camAnim.SetTrigger("shake");
        //isBack = 0.1f;
        isHit = 0.36f;

        if (hit)
        {
            GameObject go = Instantiate(blood, transform.position, Quaternion.identity, WC.WorldFolder.transform.parent) as GameObject;
            deathSound.Play();
            health--;

            if (health <= 0)
            {
                if (!isDying)
                {
                    StartCoroutine(death());
                }
                   // Controller.LoadScene(GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().sceneNum);
            }

            SetHeart();
        }
       
    }

    private bool isDying = false;
    private IEnumerator death()
    {
        isDying = true;
        canMove = false;
        deathSound.Play();
        yield return new WaitForSeconds(.4f);

        Controller.life--;

        if (Controller.life <= 0)
        {
            Controller.LoadScene(0);
        }

        else Controller.LoadScene(GameObject.FindGameObjectWithTag("GameController").GetComponent<WorldController>().sceneNum);
    }

}
