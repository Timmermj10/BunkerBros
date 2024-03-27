using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherFPV : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    private void Start()
    {
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);
    }

    void Update()
    {
        if (target)
        {
            transform.position = target.position + offset;
            transform.rotation = target.rotation;
        }
    }

    private void _PlayerRespawn(PlayerRespawnEvent e)
    {
        target = GameObject.Find("PlayerLook").transform;
    }
}
