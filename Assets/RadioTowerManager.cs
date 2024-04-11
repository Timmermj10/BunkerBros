using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioTowerManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to radio tower activation events
        EventBus.Subscribe<RadioTowerActivatedManagerEvent>(radioTowerStrength);
    }

    public void radioTowerStrength(RadioTowerActivatedManagerEvent e)
    {
        // Update the number of active radio towers
        GetComponent<HasHealth>().changeHealth(1);

        // Check if the radio strength is at it's max
        if (GetComponent<HasHealth>().currentHealth == GetComponent<HasHealth>().maxHealth)
        {
            GameObject.Find("Evacuation").GetComponent<Button>().interactable = true;
        }
    }
}
