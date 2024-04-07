using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AirdropListener : MonoBehaviour
{

    public GameObject airdropPrefab;    // Prefab for the basic airdrop
    public GameObject nukePrefab;       // Prefab for the nuke
    public GameObject missilePrefab;    // Prefab for the missile

    private float dropHeight = 10;       //How high above the ground the airdrop will fall from

    Subscription<ItemUseEvent> airdrop_event_listener;

    void Start()
    {
        airdrop_event_listener = EventBus.Subscribe<ItemUseEvent>(_Airdrop);
    }


    private void _Airdrop(ItemUseEvent e)
    {
        //If the item used is an airdrop item
        if (e.isAirdrop)
        {

            //Debug.Log("Starting Airdrop");

            GameObject prefabToInstantiate;
            Quaternion rotation;
            GameObject airdrop;

            if (e.itemID == 4) //nuke
            {
                prefabToInstantiate = nukePrefab;
                rotation = Quaternion.Euler(0, 0, 180);
                dropHeight = 15f;
            }
            else if (e.itemID == 5) //missile
            {
                prefabToInstantiate = missilePrefab;
                rotation = Quaternion.Euler(0, 0, 180);
                dropHeight = 10f;
            }
            else //any other airdrop
            {
                prefabToInstantiate = airdropPrefab;
                rotation = Quaternion.identity;
                dropHeight = 8f;
            }

            //get the airdrop start and end position
            Vector3 initialDropLocation = new Vector3(e.itemLocation.x, e.itemLocation.y + dropHeight, e.itemLocation.z);
            Vector3 finalDropLocation = initialDropLocation - new Vector3(0f, dropHeight, 0f);

            //Debug.Log($"Spawning airdrop with initialLocation of {initialDropLocation} and final location of {finalDropLocation}");

            //Instantiate the drop
            airdrop = Instantiate(prefabToInstantiate, initialDropLocation, rotation);
            EventBus.Publish(new AirDropStartedEvent(e.itemID, airdrop.transform));

            // Start the descending coroutine to handle contact with the ground
            StartCoroutine(WaitForAirdropToLand(airdrop, initialDropLocation, finalDropLocation, e.itemID));
        }
    }

    private IEnumerator WaitForAirdropToLand(GameObject airdrop, Vector3 initialDropLocation, Vector3 finalDropLocation, int itemID)
    {

        Rigidbody rb = airdrop.transform.GetComponent<Rigidbody>();

        // Set initial velocity to 0
        rb.velocity = Vector3.zero;

        while (airdrop.transform.position.y > finalDropLocation.y + 0.05)
        {
            yield return new WaitForFixedUpdate(); // Wait until the next physics update
        }

        airdrop.transform.position = initialDropLocation - new Vector3(0f, dropHeight, 0f);
        Destroy(airdrop);

        EventBus.Publish(new AirdropLandedEvent(itemID, finalDropLocation));

        //Particle effects for dusty landing
        ParticleManager.RequestDust(finalDropLocation - new Vector3(0f, 0.5f, 0f));
    }

}
