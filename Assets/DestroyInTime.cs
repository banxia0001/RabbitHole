using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInTime : MonoBehaviour
{
    public float _time;
  
    void Start()
    {
        Destroy(this.gameObject, _time);
    }


}
