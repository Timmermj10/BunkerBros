using System.IO;
using UnityEditor.Experimental.GraphView;
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

    // True/False for if the path to the object is complete
    private bool complete = true;

    // GameObject to hold the closest GameObject infront of the zombie
    private GameObject closestObject;


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
            Vector3 objectiveOffset = transform.position - objective.transform.position;

            // Initialize the player offset
            Vector3 playerOffset;

            // If we have a player in the scene
            if (player != null)
            {
                // Set the player offset to the distance between the enemy and the player
                playerOffset = transform.position - player.transform.position;
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

            // Adjust the closest point a little to make it so they all don't stack up
            if (minDistance == Vector3.zero)
            {
                Vector3 direction = minOffset - minDistance;
                float angle = Mathf.Atan2(direction.z, direction.x);

                minDistance.x += 0.5f * Mathf.Cos(angle);
                minDistance.z += 0.5f * Mathf.Sin(angle);
            }

            // Debug.Log(minDistance);

            // If the enemies are currently walking
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                // To allow movement
                agent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;

                // To start the agent
                agent.isStopped = false;

                // Need to determine if the path is complete

                // If the path is not complete we need to set the path to the nearest object blocking the path

                // Do this by sending out a raycast from the feet of the zombie toward the objective
                // Whatever it hits first, set this to the closestObject game object to be considered in the minOffset calculation

                // If the path is complete

                // Set where they are going to move to
                agent.SetDestination(minDistance);
            }
            // If the enemies are attacking
            else
            {
                // To stop the agent
                agent.isStopped = true;

                // Clear path
                agent.ResetPath();

                // To prevent movement
                agent.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

                // Angle them towards the thing they are attacking
                // Vector3 direction = minOffset - minDistance;
                // Quaternion rotation = Quaternion.LookRotation(direction);

                // transform.rotation = Quaternion.Euler(0, -rotation.eulerAngles.y, 0);
                // transform.LookAt(transform.position + minOffset);
            }
        }
    }
}
