using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform playerCamera;

    private void Start()
    {
        playerCamera = GameObject.Find("PlayerCamera").transform;
    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + playerCamera.forward);
    }
}
