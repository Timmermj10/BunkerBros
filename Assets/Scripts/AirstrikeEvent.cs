using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeEvent
{
    public float x_cord;
    public float y_cord;
    public float z_cord;

    public AirstrikeEvent(Vector3 coordinates)
    {
        x_cord = coordinates.x;
        y_cord = coordinates.y;
        z_cord = coordinates.z;
    }
}
