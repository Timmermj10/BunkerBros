using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherFPV : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    void Update()
    {
        if (target)
        {
            transform.position = target.position + offset;
            transform.rotation = target.rotation;
        }
    }
}
