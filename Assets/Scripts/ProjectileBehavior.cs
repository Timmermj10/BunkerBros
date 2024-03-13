using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float projectileSpeed = 10f;
    private Rigidbody rb;

    float despawnTimer = 3f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = projectileSpeed * transform.right;
    }

    private void OnTriggerEnter(Collider other)
    {
        rb.velocity = Vector3.zero;
        Debug.Log("Collision with " + other.gameObject.name + ": Destroying Projectile");
        Destroy(gameObject);
    }

    private void Update()
    {
        despawnTimer -= Time.deltaTime;
        if (despawnTimer < 0)
        {
            Destroy(gameObject);
        }
    }

}
