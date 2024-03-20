using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class AirdropListener : MonoBehaviour
{

    public GameObject airdropPrefab;    // Prefab for the airdrop
    private float dropHeight = 8;       //How high above the ground the airdrop will fall from

    //Defines the speed of the airdrop as it falls
    public AnimationCurve descentCurve;

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
            Debug.Log("Starting Airdrop");

            //get the airdrop start position
            Vector3 initialDropLocation = new Vector3(e.itemLocation.x, e.itemLocation.y + dropHeight, e.itemLocation.z);
            Vector3 finalDropLocation = initialDropLocation - new Vector3(0f, dropHeight, 0f);

            GameObject airdrop = Instantiate(airdropPrefab, initialDropLocation, Quaternion.identity);

            // Start the descending coroutine to apply force and maybe handle contact with the ground
            StartCoroutine(ApplyDescendingForce(airdrop, initialDropLocation, finalDropLocation, e.itemID));
        }
    }

    private IEnumerator ApplyDescendingForce(GameObject airdrop, Vector3 initialDropLocation, Vector3 finalDropLocation, int itemID)
    {
        Rigidbody rb = airdrop.GetComponent<Rigidbody>();
        Vector3 forceDirection = new Vector3(0, -9.81f, 0); // Gravity-like force

        // Condition for applying force: we haven't descended dropHeight units yet
        while (airdrop.transform.position.y > finalDropLocation.y)
        {
            rb.AddForce(forceDirection, ForceMode.Force);
            yield return new WaitForFixedUpdate(); // Wait until the next physics update to apply force again
        }

        airdrop.transform.position = initialDropLocation - new Vector3(0f, dropHeight, 0f);
        Destroy(airdrop);

        EventBus.Publish(new AirdropLandedEvent(itemID, finalDropLocation));

        //Particle effects for dusty landing
        ParticleManager.RequestDust(finalDropLocation - new Vector3(0f, 0.5f, 0f));
    }

}
