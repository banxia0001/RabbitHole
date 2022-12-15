using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    public LayerMask RockMask;
    public Sprite[] RockSps;
    // Start is called before the first frame update
    void Awake()
    {
        Collider2D[] Objs = Physics2D.OverlapCircleAll(transform.position, 2f, RockMask);
        if(Objs.Length > 0)
        {
            for(int i = 0; i < Objs.Length; i++)
            {
                if (Objs[i].gameObject != gameObject)
                {
                    Destroy(Objs[i].gameObject);
                }
            }
        }
        Debug.Log("Spawned");
        GetComponent<SpriteRenderer>().sprite = RockSps[Random.Range(0, RockSps.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
