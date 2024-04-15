using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementNavMeshNew : MonoBehaviour
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

    // Nearest destructable
    GameObject nearestDestructable;


    private void Awake()
    {
        // Get a reference to the objective, player, and animator
        objective = GameObject.Find("Objective");
        player = GameObject.Find("player");
        animator = this.GetComponent<Animator>();

        // Set the animator speed
        animator.speed = speed * 2;

        // Define all the possible locations that the zombie could attack
        DefinePossibleLocations();

        // Determine which location we will pursue
        if (objective != null)
        {
            // Start walking
            // Set the walking bool
            active = true;
            animator.SetBool("walking", active);
            
            // Set the path
            SetPath();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HasReachedDestination() && !attacking)
        {
            // Zombie has reached its destination

            // Send out raycasts in a sphere to find nearest object
            nearestDestructable = FindNearestDestructible();

            if (nearestDestructable != null)
            {
                // To stop the agent
                agent.isStopped = true;

                // Rotate our zombie toward the destructable and start attacking
                transform.LookAt(nearestDestructable.transform.position, Vector3.up);

                // Set the attacking bool
                attacking = true;
                animator.SetBool("attacking", attacking);

                // Set the walking bool
                active = false;
                animator.SetBool("walking", active);

                // Clear path
                agent.ResetPath();
            }
            else
            {
                // Reset the path
                SetPath();
            }
        }

        else if (HasReachedDestination() && attacking && player != null && (nearestDestructable.transform.position - transform.position).magnitude > agent.stoppingDistance)
        {
            // To stop the agent
            agent.isStopped = false;

            // Set the attacking bool
            attacking = false;
            animator.SetBool("attacking", attacking);

            // Set the walking bool
            active = true;
            animator.SetBool("walking", active);

            // Reset the path
            SetPath();
        }

        // If we are attacking and the nearestDestructable is null
        else if (attacking && nearestDestructable == null)
        {
            // Reset the path
            SetPath();

            // Set attacking to false
            attacking = false;
            animator.SetBool("attacking", attacking);

            // Set walking to true
            active = true;
            animator.SetBool("walking", active);
        }

        // If we aren't attacking and the nearestDestrutable is null
        // Need to check if the zombie is closer to the player than the objective
        else if (!attacking && isPlayerCloser())
        {
            // Reset the path
            SetPath();
        }
    }

    private bool HasReachedDestination()
    {
        // Check if we have a valid target to move to.
        if (target == Vector3.zero)
            return false;

        // Check if path is pending, i.e., the agent is still calculating the route.
        if (agent.pathPending)
            return false;

        Debug.Log(agent.remainingDistance);
        // Check if the remaining distance is less than the stopping distance.
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Make sure the agent isn't moving to the next point along the path.
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                return true;
            }
        }

        return false;
    }

    private GameObject FindNearestDestructible()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Structure") | LayerMask.GetMask("Player"));
        GameObject nearestDestructible = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider hitCollider in hitColliders)
        {
            Debug.Log(hitCollider);
            GameObject destructible = hitCollider.gameObject;

            Vector3 directionToTarget = hitCollider.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                nearestDestructible = destructible;
            }
        }

        return nearestDestructible;
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

    private void SetPath()
    {
        // Determine which location we will pursue
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

            if (player != null && isPlayerCloser())
            {
                target = player.transform.position;
            }
            else
            {
                DetermineBestLocation();
            }

            agent.SetDestination(target);
        }
    }

    private bool isPlayerCloser()
    {
        // Determine which location we will pursue
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
                // Try to find the player
                player = GameObject.Find("player");

                // Set the player offset to infinity (so the enemy won't go after an imaginary player)
                playerOffset = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }

            bool val = objectiveOffset.magnitude <= playerOffset.magnitude ? false : true;

            // Distance to work with NavMesh
            return objectiveOffset.magnitude <= playerOffset.magnitude ? false : true;
        }

        return false;
    }
}
