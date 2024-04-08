using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioTowerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to radio tower activation events
        EventBus.Subscribe<RadioTowerActivatedEvent>(radioTowerStrength);
    }

    public void radioTowerStrength(RadioTowerActivatedEvent e)
    {
        // Update the number of active radio towers
        GetComponent<HasHealth>().changeHealth(1);
    }
}
