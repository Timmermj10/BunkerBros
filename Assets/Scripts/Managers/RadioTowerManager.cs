using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class RadioTowerManager : MonoBehaviour
{
    // Holds the currently activated transformer
    private GameObject currentActiveTransformer;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to radio tower activation events
        EventBus.Subscribe<RadioTowerActivatedPlayerEvent>(miniGameStart);
        EventBus.Subscribe<RadioTowerActivatedManagerEvent>(radioTowerStrength);
        EventBus.Subscribe<miniGameAbortEvent>(abortMiniGame);
    }

    public void miniGameStart(RadioTowerActivatedPlayerEvent e)
    {
        if (gameObject.name is "MiniGame")
        {
            currentActiveTransformer = e.transformerInteractable;
            gameObject.transform.Find("GrayBackground").gameObject.SetActive(true);
        }
    }


    public void radioTowerStrength(RadioTowerActivatedManagerEvent e)
    {
        // If we are working with signal strength
        if (gameObject.name is "SignalStrength")
        {
            // Update the number of active radio towers
            GetComponent<HasHealth>().changeHealth(1);

            Debug.Log(GetComponent<SignalStages>().sprites[0].name);

            // Update the sprite
            GetComponent<Image>().sprite = GetComponent<SignalStages>().sprites[GetComponent<HasHealth>().currentHealth];

            // Check if the radio strength is at it's max
            if (GetComponent<HasHealth>().currentHealth == GetComponent<HasHealth>().maxHealth)
            {
                GameObject.Find("Evacuation").GetComponent<Button>().interactable = true;
            }
        }
        // If we are working with the minigame
        else
        {
            GameObject.Find("GrayBackground").gameObject.SetActive(false);
        }
    }

    public void abortMiniGame(miniGameAbortEvent e)
    {
        if (gameObject.name is "MiniGame")
        {
            // Update the color on the transformer
            ChangeMaterial changeMaterialScript = currentActiveTransformer.transform.parent.GetComponent<ChangeMaterial>();
            bool enable = false;
            if (changeMaterialScript != null)
            {
                changeMaterialScript.ChangeKnobMaterial(currentActiveTransformer.transform.parent.gameObject, enable);
            }
            else
            {
                Debug.LogWarning("ChangeMaterial script not found on item: " + currentActiveTransformer.name);
            }

            // Make it so you can interact with the same radio tower
            currentActiveTransformer.tag = "Interactable";

            // Turn off minigame
            GameObject.Find("GrayBackground").gameObject.SetActive(false);
        }
    }
}
