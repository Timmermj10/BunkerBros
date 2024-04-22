using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lockGlobalRot : MonoBehaviour
{
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.identity;
    }
}
