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
    public void UnPing()
    {
        foreach (Transform x in transform)
        {
            if (x.CompareTag("Ping"))
            {
                x.gameObject.SetActive(false);
            }
        }
    }
    public void TogglePing()
    {
        foreach (Transform x in transform)
        {
            if (x.CompareTag("Ping"))
            {
                x.gameObject.SetActive(!x.gameObject.activeSelf);
            }
        }
    }
}
