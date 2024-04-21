using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundPoint : MonoBehaviour
{
    public Transform target; // The point around which the camera will rotate
    public float rotationSpeed = 5f; // Speed of rotation

    void Update()
    {
        // Check if the target is set
        if (target != null)
        {
            // Rotate the camera around the target
            transform.RotateAround(target.position, Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}
