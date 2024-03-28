using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKitUse : MonoBehaviour
{
    public int repair_value = 5;

    public void UseKit()
    {
        if (GetComponent<HasHealth>() != null && gameObject.CompareTag("Objective"))
        {
            GetComponent<HasHealth>().increaseHealth(repair_value);
        }
    }


}
