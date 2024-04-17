using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetBombPlaneController : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.right * speed;
    }

    void Update()
    { 
        //if not playing plane sound, play plane sound
    }
}
