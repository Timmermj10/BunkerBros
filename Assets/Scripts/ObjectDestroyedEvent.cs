using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyedEvent
{
    public string name;
    public string tag;
    public Vector3 deathCoordinates;

    public ObjectDestroyedEvent(string name, string tag, Vector3 deathCoordinates)
    {
        this.name = name;
        this.tag = tag;
        this.deathCoordinates = deathCoordinates;
    }

}
