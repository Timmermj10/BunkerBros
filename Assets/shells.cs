using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shells : MonoBehaviour
{
    public bool trigger;
    ParticleSystem ps;
    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if(trigger)
        {
            ps.Emit(1);
            trigger = false;
        }
    }
}
