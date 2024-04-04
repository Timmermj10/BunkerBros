using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{

    public float speed = 5f;
    //public float moveDistance = 10f;
    public float attackDistance = 1.5f;

    Subscription<PlayerRespawnEvent> respawn_event_subscription;
    Subscription<PopUpStartEvent> startpopup_subscription;
    Subscription<PopUpEndEvent> endpopup_subscription;
    private GameObject objective;
    private GameObject player;
    private bool active = false;
    private bool attacking = false;
    private Animator animator;
    private Rigidbody rb;

    private bool canMove = true;

    void Awake()
    {
        objective = GameObject.Find("Objective");
        player = GameObject.Find("player");
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        animator.speed = speed * 2;
        respawn_event_subscription = EventBus.Subscribe<PlayerRespawnEvent>(_ResetPlayer);
        startpopup_subscription = EventBus.Subscribe<PopUpStartEvent>(_FreezeMovement);
        endpopup_subscription = EventBus.Subscribe<PopUpEndEvent>(_UnfreezeMovement);
    }
    public void _ResetPlayer(PlayerRespawnEvent e)
    {
        player = e.activePlayer;
    }

    private void _FreezeMovement(PopUpStartEvent e)
    {
        canMove = false;
    }

    private void _UnfreezeMovement(PopUpEndEvent e)
    {
        canMove = true;
    }

    void Update()
    {
        if (objective != null && canMove)
        {
            Vector3 objectiveOffset = objective.transform.position - transform.position;
            Vector3 playerOffset;
            if (player != null)
            {
                playerOffset = player.transform.position - transform.position;
            } else
            {
                playerOffset = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }
            Vector3 minOffset = objectiveOffset.magnitude <= playerOffset.magnitude ? objectiveOffset : playerOffset;
            minOffset.y = 0;
            active = minOffset.magnitude > attackDistance;
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
        } else
        {
            rb.velocity = Vector3.zero;
        }
    }


    private void OnDestroy()
    {
        EventBus.Unsubscribe(respawn_event_subscription);
        EventBus.Unsubscribe(startpopup_subscription);
        EventBus.Unsubscribe(endpopup_subscription);
    }


}


