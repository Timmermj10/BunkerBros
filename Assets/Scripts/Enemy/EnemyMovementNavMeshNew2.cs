using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovementNavMeshNew2 : MonoBehaviour
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

            agent.SetDestination(target);
        }
    }


}
