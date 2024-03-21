using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldChestPublisher : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name is "player")
        {
            EventBus.Publish<GoldChestEvent>(new GoldChestEvent(true));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name is "player")
        {
            EventBus.Publish<GoldChestEvent>(new GoldChestEvent(false));
        }
    }
}
