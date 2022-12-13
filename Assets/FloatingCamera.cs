using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCamera : MonoBehaviour
{
    public Transform target;
    public float smoothing;
    void Update()
    {
        if (target != null)
        {
            if (transform.position != target.position)
            {
                Vector3 targetPos = target.position;
                transform.position = Vector3.Lerp(transform.position, targetPos, smoothing);
            }
        }
    }
}
