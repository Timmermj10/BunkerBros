using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyedScript : MonoBehaviour
{
    // When the object is destroyed, send out a object destroyed event
    private void OnDestroy()
    {
        EventBus.Publish(new ObjectDestroyedEvent(gameObject.name, gameObject.tag, gameObject.transform.position));
    }
}
