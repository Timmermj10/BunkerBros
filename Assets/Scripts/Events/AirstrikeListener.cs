using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AirstrikeListener : MonoBehaviour
{
    // List to hold all of the loaded silos
    public List<MissileSiloStatus> siloStatus = new List<MissileSiloStatus>();

    public GameObject explosionPrefab;    // Prefab for the explosion effect
    public GameObject missleExplosionPrefab; // Prefab for the missle explosion
    private float blastRadius = 5f;        // Radius of the nuke's effect
    public LayerMask damageableLayer;     // Layers that can be damaged by the nuke, set up in the inspector
    private int explosionDamage = -40;    //Amount of damage for the explosion

    private int nukeDamage = -40;
    private int missileDamage = -20;


    // Subscribe to Purchase Events
    Subscription<ItemUseEvent> nuke_event_subscription;

    // Subscribe to Silo Loaded Events
    Subscription<SiloLoadedEvent> silo_loaded_event_subscription;

    // Subscribe to AirdropLanded Events
    Subscription<AirdropLandedEvent> airdrop_landed_subscription;

    // Start is called before the first frame update
    void Start()
    {
        nuke_event_subscription = EventBus.Subscribe<ItemUseEvent>(_CallNuke);
        silo_loaded_event_subscription = EventBus.Subscribe<SiloLoadedEvent>(_SiloLoadedStrike);
        airdrop_landed_subscription = EventBus.Subscribe<AirdropLandedEvent>(_Explode);
    }

    void _CallNuke(ItemUseEvent e)
    {
        // if the id is the nukeID
        if (e.itemID == 4 && siloStatus.Count > 0)
        {

            //Call in an nuke at the itemLocation
            //StartCoroutine(DelayedExplosion(e.itemLocation));

            // Unload the silo
            siloUnloadedStrike();
        }
    }

    private void _Explode(AirdropLandedEvent e)
    {
        if (e.itemID == 4) //nuke
        {
            blastRadius = 5;
            explosionDamage = nukeDamage;
            StartCoroutine(ExplosionEffect(e.itemLocation, explosionPrefab));
        }
        else if (e.itemID == 5) //missile
        {
            blastRadius = 1.5f;
            explosionDamage = missileDamage;
            StartCoroutine(ExplosionEffect(e.itemLocation, missleExplosionPrefab));
        }
    }

    private IEnumerator ExplosionEffect(Vector3 position, GameObject explosionPrefab)
    {

        // Create the explosion effect at the height-adjusted position
        GameObject explosionEffect = Instantiate(explosionPrefab, position, Quaternion.identity);

        // Get the ParticleSystem component
        ParticleSystem particles = explosionEffect.GetComponent<ParticleSystem>();

        //Debug.Log("Calling DamageObjectsWithinRadius");
        DamageObjectsWithinRadius(position, blastRadius, explosionDamage);


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
        }

        // Damage each object
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

        // Publish that the silo was unloaded
        EventBus.Publish<SiloUnloadedEvent>(new SiloUnloadedEvent());
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
