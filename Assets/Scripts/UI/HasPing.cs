using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasPing : MonoBehaviour
{
    public void Ping()
    {
        foreach (Transform x in transform)
        {
            if (x.CompareTag("Ping"))
            {
                x.gameObject.SetActive(true);
            }
        }
    }
}
