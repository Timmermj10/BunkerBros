using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyMovementNavMeshTest : MonoBehaviour
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

    // Hold the center of where we are currently going
    Vector3 targetCenter = Vector3.zero;

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
        if (player != null)
        {
            DetermineBestLocation(player.transform.position);
        }
        else
        {
            DetermineBestLocation(new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity));
        }
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
                // Try to find the player
                player = GameObject.Find("player");

                // Set the player offset to infinity (so the enemy won't go after an imaginary player)
                playerOffset = new Vector3(Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }

            // Offset to work with animator
            Vector3 minOffset = objectiveOffset.magnitude <= playerOffset.magnitude ? objectiveOffset : playerOffset;
            minOffset.y = 0;

            // Distance to work with NavMesh
            minDistance = objectiveOffset.magnitude <= playerOffset.magnitude ? objective.transform.position : player.transform.position;
            minDistance.y = 0;

            // If the min distance is different than the target
            if (minDistance != targetCenter || player == null)
            {
                //Debug.Log("here");
                if (player != null)
                {
                    DetermineBestLocation(player.transform.position);
                }
                else
                {
                    DetermineBestLocation(playerOffset);
                }
            }

            targetCenter = minDistance;



            //// Adjust the closest point a little to make it so they all don't stack up
            //if (minDistance == Vector3.zero)
            //{
            //    Vector3 direction = minOffset - minDistance;
            //    float angle = Mathf.Atan2(direction.z, direction.x);

            //    minDistance.x += 0.5f * Mathf.Cos(angle);
            //    minDistance.z += 0.5f * Mathf.Sin(angle);
            //}

            // DefinePossibleLocations();
            //Debug.Log(target);

            active = minOffset.magnitude > attackDistance;
            animator.SetBool("walking", active);
            //attacking = minOffset.magnitude <= attackDistance;
            //DetermineBestLocation();
            // If the agent has an end point, check if we are close enough to the end point to start attacking
            if (agent.remainingDistance > 0)
            {
                attacking = agent.remainingDistance < attackDistance;
                // Check if we are close enough to the objective to override it
                // Debug.Log((transform.position - Vector3.zero).magnitude);
                if ((transform.position - Vector3.zero).magnitude < 1.5)
                {
                    //Debug.Log("Close to objective");
                    attacking = true;
                }
            }
            animator.SetBool("attacking", attacking);

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
                // Send out raycasts in a sphere to find nearest object
                nearestDestructable = FindNearestDestructible();

                if (nearestDestructable != null)
                {
                    Vector3 look = new Vector3(nearestDestructable.transform.position.x, transform.position.y, nearestDestructable.transform.position.z);
                    // Rotate our zombie toward the destructable and start attacking
                    transform.LookAt(look, Vector3.up);

                    // To stop the agent
                    agent.isStopped = true;

                    // Clear path
                    agent.ResetPath();
                }
                else
                {
                    attacking = false;
                    animator.SetBool("attacking", attacking);
                }

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

    private Vector3 DetermineBestLocation(Vector3 playerLoc)
    {
        // Float to hold the destination that is closest to the enemy currently
        float closestToEnemy = Mathf.Infinity;
        Vector3 position = Vector3.zero;

        // Get a random angle
        float angle = Random.Range(0f, Mathf.PI * 2f);

        // Convert polar to cartesian coordinates
        float x = Mathf.Cos(angle);
        float z = Mathf.Sin(angle);

        position = new Vector3(x, 0, z);
        closestToEnemy = (transform.position - position).magnitude;

        // If the player is closer
        if ((transform.position - playerLoc).magnitude < closestToEnemy)
        {
            target = playerLoc;
            return target;
        }
        // If we have somewhere we can go
        else if (closestToEnemy != Mathf.Infinity)
        {
            target = position;
            return target;
        }

        // If no valid paths are found to possible locations, find the nearest obstacle
        target = Vector3.zero;
        return target;
    }

    //private Vector3 DetermineBestLocation(Vector3 playerLoc)
    //{
    //    // Float to hold the destination that is closest to the enemy currently
    //    float closestToEnemy = Mathf.Infinity;
    //    Vector3 position = Vector3.zero;

    //    // Loop through the locations
    //    foreach (Vector3 location in possibleLocations)
    //    {
    //        NavMeshPath path = new NavMeshPath();
    //        if (agent.CalculatePath(location, path) && path.status == NavMeshPathStatus.PathComplete)
    //        {
    //            // If a complete path is found, check if the location is closer than the current to the enemy
    //            if ((transform.position - location).magnitude < closestToEnemy)
    //            {
    //                closestToEnemy = (transform.position - location).magnitude;
    //                position = location;
    //            }
    //        }
    //    }

    //    // If the player is closer
    //    if ((transform.position - playerLoc).magnitude < closestToEnemy)
    //    {
    //        target = playerLoc;
    //        return target;
    //    }
    //    // If we have somewhere we can go
    //    else if (closestToEnemy != Mathf.Infinity)
    //    {
    //        target = position;
    //        return target;
    //    }


    //    // If no valid paths are found to possible locations, find the nearest obstacle
    //    target = Vector3.zero;
    //    return target;
    //}

    private GameObject FindNearestDestructible()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1, LayerMask.GetMask("Structure") | LayerMask.GetMask("Player"));
        GameObject nearestDestructible = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;

        foreach (Collider hitCollider in hitColliders)
        {
            //Debug.Log(hitCollider);
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
