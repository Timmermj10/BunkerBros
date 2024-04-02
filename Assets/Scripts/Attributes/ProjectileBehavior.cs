using UnityEngine;

public class ProjectileBehavior : MonoBehaviour
{
    private float projectileSpeed = 15f;
    private Rigidbody rb;

    float despawnTimer = 3f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = projectileSpeed * transform.right;
    }

    private void OnTriggerEnter(Collider other)
    {
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
