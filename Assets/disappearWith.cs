using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class disappearWith : MonoBehaviour
{
    public GameObject target;
    private void OnEnable()
    {
        target.SetActive(true);
    }
    private void OnDisable()
    {
        target.SetActive(false);
    }
}
