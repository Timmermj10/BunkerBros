using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetherFPV : MonoBehaviour
{
    private Transform target;
    public Vector3 offset;


    private void Start()
    {
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);

        target = GameObject.FindWithTag("Player").transform;
        
        while (target.parent != null)
        {
            target = target.parent;
        }
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
        target = e.activePlayer.transform;
    }
}
