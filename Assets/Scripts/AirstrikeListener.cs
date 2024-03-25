using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AirstrikeListener : MonoBehaviour
{
    // List to hold all of the loaded silos
    public List<MissileSiloStatus> siloStatus = new List<MissileSiloStatus>();

    public GameObject explosionPrefab;    // Prefab for the explosion effect
    public float nukeHeight = 2f;   // Height from which the nuke comes
    public float blastRadius = 3f;        // Radius of the nuke's effect
    public LayerMask damageableLayer;     // Layers that can be damaged by the nuke, set up in the inspector
    private int nukeDamage = -15;


    // Subscribe to Purchase Events
    Subscription<ItemUseEvent> nuke_event_subscription;

    // Subscribe to Silo Loaded Events
    Subscription<SiloLoadedEvent> silo_loaded_event_subscription;

    // Start is called before the first frame update
    void Start()
    {
        nuke_event_subscription = EventBus.Subscribe<ItemUseEvent>(_CallNuke);
        silo_loaded_event_subscription = EventBus.Subscribe<SiloLoadedEvent>(_SiloLoadedStrike);
    }

    void _CallNuke(ItemUseEvent e)
    {
        // if the id is the nukeID
        if (e.itemID == 4)
        {
            //Call in an nuke at the itemLocation
            StartCoroutine(DelayedExplosion(e.itemLocation));

            // Unload the silo
            siloUnloadedStrike();
        }
    }

    private IEnumerator DelayedExplosion(Vector3 position)
    {
        // Wait for a delay to simulate time it takes for nuke to arrive
        // yield return new WaitForSeconds(3f); // 3 second delay for the effect

        // Create the explosion effect at the height-adjusted position
        GameObject explosionEffect =  Instantiate(explosionPrefab, position, Quaternion.identity);

        // Get the ParticleSystem component
        ParticleSystem particles = explosionEffect.GetComponent<ParticleSystem>();

        //Debug.Log("Calling DamageObjectsWithinRadius");
        DamageObjectsWithinRadius(position, blastRadius, nukeDamage);


        // Wait for the particle system to finish
        yield return new WaitForSeconds(particles.main.duration);

        // Destroy the GameObject that holds the particle system
        Destroy(explosionEffect);
    }

    void DamageObjectsWithinRadius(Vector3 center, float radius, int damage)
    {
        // Find all colliders within the radius at the center point.
        // You can also provide a LayerMask to filter colliders by layer.
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);

        // Optionally, filter hitColliders by tag or by checking component types
        List<HasHealth> objectsToDamage = new List<HasHealth>();
        foreach (Collider hitCollider in hitColliders)
        {
            HasHealth hasHealth = hitCollider.GetComponent<HasHealth>();
            if (hasHealth != null && !objectsToDamage.Contains(hasHealth))
            {
                objectsToDamage.Add(hasHealth);
            }
            // Or check for a certain component, like so:
            // if (hitCollider.GetComponent<YourComponentType>() != null)
            // {
            //     nearbyObjects.Add(hitCollider.gameObject);
            // }
        }

        // Do something with the objects you found
        foreach (HasHealth obj in objectsToDamage)
        {
            //Debug.Log("Found an object with health: " + obj.name);
            obj.changeHealth(damage);
        }
    }

    void _SiloLoadedStrike(SiloLoadedEvent e)
    {
        // Add the silo to the list of loaded silos
        siloStatus.Add(e.status);
    }

    void siloUnloadedStrike()
    {
        // Unload silo for the first silo in list
        siloStatus[0].unloadSilo();

        // Pop the silo from the list
        siloStatus.RemoveAt(0);
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space to test the nuke
        {
            Vector3 target = new Vector3(0, 0, 0); // Replace with your target coordinates
            _Callnuke(new nukeEvent(target));
        }
    }
    */
}
