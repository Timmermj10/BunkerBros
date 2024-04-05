using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementNavMesh : MonoBehaviour
{
    // Speed of the enemies
    public float speed = 5f;

    // Float for how far away from something they initiate their attack
    public float attackDistance = 1.5f;

    // GameObjects for the objective and player
    private GameObject objective;
    private GameObject player;

    // Bools for if the enemy is walking or attacking
    private bool active = false;
    private bool attacking = false;

    // Animation for the enemies
    private Animator animator;

    // NavMesh Agent
    public NavMeshAgent agent;


    private void Awake()
    {
        objective = GameObject.Find("Objective");
        player = GameObject.Find("player");
        animator = this.GetComponent<Animator>();
        animator.speed = speed * 2;
    }

    // Update is called once per frame
    void Update()
    {
        if (objective != null)
        {
            // Get the objective offset
            Vector3 objectiveOffset = objective.transform.position - transform.position;

            // Initialize the player offset
            Vector3 playerOffset;

            // If we have a player in the scene
            if (player != null)
            {
                // Set the player offset to the distance between the enemy and the player
                playerOffset = player.transform.position - transform.position;
            }
            else
            {
                // Set the player offset to infinity (so the enemy won't go after an imaginary player)
                playerOffset = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }

            // Offset to work with animator
            Vector3 minOffset = objectiveOffset.magnitude <= playerOffset.magnitude ? objectiveOffset : playerOffset;
            minOffset.y = 0;

            // Distance to work with NavMesh
            Vector3 minDistance = objectiveOffset.magnitude <= playerOffset.magnitude ? objective.transform.position : player.transform.position;
            minDistance.y = 0;

            active = minOffset.magnitude > attackDistance;
            animator.SetBool("walking", active);
            attacking = minOffset.magnitude <= attackDistance;
            animator.SetBool("attacking", attacking);
            // transform.LookAt(transform.position + minOffset);

            // Debug.Log(minDistance);

            // If we are currently walking
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                // Set where they are going to move to
                agent.SetDestination(minDistance);
            }
            // If we are ???
            else
            {
                // Set where they are going to move to
                // agent.SetDestination(minDistance);
            }
        }
    }
}
