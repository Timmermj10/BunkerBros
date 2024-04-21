using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallVictoryMusic : MonoBehaviour
{
    void Start()
    {
        EventBus.Publish(new VictoryMusicEvent());
    }
}
