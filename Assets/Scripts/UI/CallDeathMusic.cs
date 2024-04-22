using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallDeathMusic : MonoBehaviour
{
    void Start()
    {
        EventBus.Publish(new DeathMusicEvent());
    }
}
