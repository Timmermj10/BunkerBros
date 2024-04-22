using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class destroyOnLand : MonoBehaviour
{
    // Destroy 1 second after being created
    void Start()
    {
        // Destroy this game object after 1 second
        Destroy(gameObject, 1f);
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    Destroy(gameObject);
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    Destroy(gameObject);
    //}
}
