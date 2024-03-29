using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{

    public float speed = 5f;
    public float moveDistance = 10f;
    public float attackDistance = 1.5f;

    Subscription<PlayerRespawnEvent> respawn_event_subscription;
    private GameObject objective;
    private GameObject player;
    private bool active = false;
    private bool attacking = false;
    private Animator animator;
    private Rigidbody rb;

    void Awake()
    {
        objective = GameObject.Find("Objective");
        player = GameObject.Find("player");
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        animator.speed = speed * 2;
        respawn_event_subscription = EventBus.Subscribe<PlayerRespawnEvent>(_ResetPlayer);
    }
    public void _ResetPlayer(PlayerRespawnEvent e)
    {
        player = e.activePlayer;
    }
    // Update is called once per frame
    void Update()
    {
        if (player == null)
        {
            Vector3 objectiveOffset = objective.transform.position - transform.position;
            Vector3 minOffset = objectiveOffset;
            active = minOffset.magnitude <= moveDistance;
            animator.SetBool("walking", active);
            attacking = minOffset.magnitude <= attackDistance;
            animator.SetBool("attacking", attacking);
            transform.LookAt(transform.position + minOffset);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                rb.velocity = minOffset.normalized * speed + Vector3.up * rb.velocity.y;
            }
            else
            {
                rb.velocity = Vector3.up * rb.velocity.y;
            }
        }
        else if (objective != null)
        {
            Vector3 objectiveOffset = objective.transform.position - transform.position;
            Vector3 playerOffset = player.transform.position - transform.position;
            Vector3 minOffset = objectiveOffset.magnitude <= playerOffset.magnitude ? objectiveOffset : playerOffset;
            minOffset.y = 0;
            active = minOffset.magnitude <= moveDistance;
            animator.SetBool("walking", active);
            attacking = minOffset.magnitude <= attackDistance;
            animator.SetBool("attacking", attacking);
            transform.LookAt(transform.position + minOffset);
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                rb.velocity = minOffset.normalized * speed + Vector3.up * rb.velocity.y;
            }
            else
            {
                rb.velocity = Vector3.up * rb.velocity.y;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Untagged")
        {
            //Debug.Log($"trigger enter: {gameObject.name} and {other.tag}");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.tag != "Untagged")
        {
            //Debug.Log($"trigger exit: {gameObject.name} and {other.tag}");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Untagged")
        {
            //Debug.Log($"trigger exit: {gameObject.name} and {collision.gameObject.tag}");
        }
    }
    private void OnDestroy()
    {
        EventBus.Unsubscribe(respawn_event_subscription);
    }
}


