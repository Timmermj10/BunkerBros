using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirstrikeListener : MonoBehaviour
{
    public GameObject explosionPrefab;    // Prefab for the explosion effect
    public float airstrikeHeight = 2f;   // Height from which the airstrike comes
    public float blastRadius = 3f;        // Radius of the airstrike's effect
    public LayerMask damageableLayer;     // Layers that can be damaged by the airstrike, set up in the inspector


    // Subscribe to Purchase Events
    Subscription<AirstrikeEvent> airstrike_event_subscription;

    // Start is called before the first frame update
    void Start()
    {
        airstrike_event_subscription = EventBus.Subscribe<AirstrikeEvent>(_CallAirstrike);
    }

    void _CallAirstrike(AirstrikeEvent e)
    {
        Vector3 strikePosition = new Vector3(e.x_cord, e.y_cord + 2f, e.z_cord);

        StartCoroutine(DelayedExplosion(strikePosition));
    }

    private IEnumerator DelayedExplosion(Vector3 position)
    {
        // Wait for a delay to simulate time it takes for airstrike to arrive
        // yield return new WaitForSeconds(3f); // 3 second delay for the effect

        // Create the explosion effect at the height-adjusted position
        GameObject explosionEffect =  Instantiate(explosionPrefab, position, Quaternion.identity);

        // Get the ParticleSystem component
        ParticleSystem particles = explosionEffect.GetComponent<ParticleSystem>();

        // Wait for the particle system to finish
        yield return new WaitForSeconds(particles.main.duration);

        // Destroy the GameObject that holds the particle system
        Destroy(explosionEffect);

        // Apply damage or effects to the target area
        Collider[] hitColliders = Physics.OverlapSphere(position, blastRadius, damageableLayer);
        foreach (var hitCollider in hitColliders)
        {
            // Apply damage to the objects within the blast radius
            // You might need to access an 'IDamageable' interface or a script on these objects that can receive damage.
            // Example: hitCollider.gameObject.GetComponent<IDamageable>()?.TakeDamage(damageAmount);

            // For demonstration purposes, let's just print out the objects hit
            Debug.Log($"Airstrike hit: {hitCollider.gameObject.name}");
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // Press Space to test the airstrike
        {
            Vector3 target = new Vector3(0, 0, 0); // Replace with your target coordinates
            _CallAirstrike(new AirstrikeEvent(target));
        }
    }
    */
}
