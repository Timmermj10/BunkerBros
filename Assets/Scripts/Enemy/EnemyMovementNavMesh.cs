using System.Collections.Generic;
using System.IO;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

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
    //private bool complete = true;

    // GameObject to hold the closest GameObject infront of the zombie
    private GameObject closestObject;

    // List of eight possible locations that the zombies can go to 
    public List<Vector3> possibleLocations;

    // Hold the min distance
    Vector3 minDistance = Vector3.zero;

    // Hold where we are currently going to 
    Vector3 target = Vector3.zero;


    private void Awake()
    {
        objective = GameObject.Find("Objective");
        player = GameObject.Find("player");
        animator = this.GetComponent<Animator>();
        animator.speed = speed * 2;

        DefinePossibleLocations();
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
            minDistance = objectiveOffset.magnitude <= playerOffset.magnitude ? objective.transform.position : player.transform.position;
            minDistance.y = 0;

            //// Adjust the closest point a little to make it so they all don't stack up
            //if (minDistance == Vector3.zero)
            //{
            //    Vector3 direction = minOffset - minDistance;
            //    float angle = Mathf.Atan2(direction.z, direction.x);

            //    minDistance.x += 0.5f * Mathf.Cos(angle);
            //    minDistance.z += 0.5f * Mathf.Sin(angle);
            //}

            DefinePossibleLocations();
            //Debug.Log(target);

            active = minOffset.magnitude > attackDistance;
            animator.SetBool("walking", active);
            //attacking = minOffset.magnitude <= attackDistance;
            attacking = (transform.position - DetermineBestLocation()).magnitude < attackDistance;
            animator.SetBool("attacking", attacking);
            // transform.LookAt(transform.position + minOffset);

            // If the enemies are currently walking
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
            {
                // To start the agent
                agent.isStopped = false;

                // Need to determine if the path is complete

                // If the path is not complete we need to set the path to the nearest object blocking the path

                // Do this by sending out a raycast from the feet of the zombie toward the objective
                // Whatever it hits first, set this to the closestObject game object to be considered in the minOffset calculation

                // If the path is complete

                // Set where they are going to move to
                // agent.SetDestination(minDistance);
                agent.SetDestination(target);
            }
            // If the enemies are attacking
            else
            {
                // To stop the agent
                agent.isStopped = true;

                // Clear path
                agent.ResetPath();

                // Angle them towards the thing they are attacking
                // Vector3 direction = minOffset - minDistance;
                // Quaternion rotation = Quaternion.LookRotation(direction);

                // transform.rotation = Quaternion.Euler(0, -rotation.eulerAngles.y, 0);
                // transform.LookAt(transform.position + minOffset);
            }
        }
    }

    private void DefinePossibleLocations()
    {
        possibleLocations = new List<Vector3>();

        // Define positions around the target, e.g., at different angles and distances
        for (int angle = 0; angle < 360; angle += 45) // Example: every 45 degrees
        {
            Vector3 direction = (Quaternion.Euler(0, angle, 0) * Vector3.forward).normalized;
            float distance = 0.75f;
            Vector3 possibleLocation = minDistance + (direction * distance);
            possibleLocations.Add(possibleLocation);
        }
    }

    private Vector3 DetermineBestLocation()
    {
        // Float to hold the destination that is closest to the enemy currently
        float closestToEnemy = Mathf.Infinity;
        Vector3 position = Vector3.zero;

        // Loop through the locations
        foreach (Vector3 location in possibleLocations)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(location, path) && path.status == NavMeshPathStatus.PathComplete)
            {
                // If a complete path is found, check if the location is closer than the current to the enemy
                if ((transform.position - location).magnitude < closestToEnemy)
                {
                    closestToEnemy = (transform.position - location).magnitude;
                    position = location;
                }
            }
        }

        // If we have somewhere we can go
        if (closestToEnemy != Mathf.Infinity)
        {
            target = position;
            return position;
        }

        // If no valid paths are found to possible locations, find the nearest obstacle
        return DetermineNearestObstacle();
    }

    private Vector3 DetermineNearestObstacle()
    {
        // Path is not complete, use raycast to find nearest obstacle
        NavMeshHit hit;
        if (NavMesh.Raycast(transform.position, minDistance, out hit, NavMesh.AllAreas))
        {
            // Get the hit position
            Vector3 position = hit.position;

            // Set the destination
            target = position;

            return position;
        }
        return Vector3.zero;
    }
}
