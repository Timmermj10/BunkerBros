using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class GoldChestEventListener : MonoBehaviour
{
    // Subscribe to Gold Chest Events
    Subscription<GoldChestEvent> gold_chest_event_subscription;

    // Timer for when the player leaves the zone, the enemies still
    // follow for a random number of seconds between 2-4 seconds
    public float timer = 2f;

    // To determine if the timer should be running
    public bool startTimer = false;

    // Enemy rigid body
    private Rigidbody enemyRB;

    // Starting position of the enemy
    public Vector3 startingPosition;

    // Enemy velocity for returning to position
    public float enemyVelocityReturn = 0.1f;

    // Enemy velocity for chasing player
    public float enemyVelocityChase = 0.1f;

    // Player transform
    private Transform playerTransform;

    // Bool for whether the enemies are meant to follow the player
    public bool followPlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to the event
        gold_chest_event_subscription = EventBus.Subscribe<GoldChestEvent>(_TrackPlayer);

        // Get the starting position of the enemy
        startingPosition = transform.position;

        // Get the player transform
        playerTransform = GameObject.Find("player").transform;

        // Get the rigidbody of the enemy
        enemyRB = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // If the timer is started, decrease the time
        if (startTimer && timer > 0)
        {
            timer -= Time.deltaTime;
        }

        if (timer <= 0)
        {
            // Stop the timer
            startTimer = false;

            // Stop following the player
            followPlayer = false;

            // Start the coroutine of returning to base
            StartCoroutine(returnToStartPosition());
        }

        else if (followPlayer)
        {
            // Find the direction the enemies need to go to get to the player
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            Debug.Log($"Direction  = {directionToPlayer}");
            // Update the transform to go that way
            enemyRB.velocity = directionToPlayer * enemyVelocityChase * Time.deltaTime;
            Debug.Log($"Velocity = {enemyRB.velocity}");
        }
        
    }

    void _TrackPlayer(GoldChestEvent e) 
    {
        if (e.entering)
        {
            // Randomize how long they will follow after leaving the box collider
            randomizeTimer();

            // Start following the player
            followPlayer = true;
        }
        else
        {
            // Start the timer
            startTimer = true;
        }
    }

    private void randomizeTimer()
    {
        timer = Random.Range(2.0f, 4.0f);
    }

    IEnumerator returnToStartPosition()
    {
        // While the object is not at the target position

        // TODO MAKE IT SO WHEN IT GETS WITHIN A CERTAIN MARGIN IT SNAPS TO POSITION AND ENDS THE LOOP

        while (transform.position != startingPosition)
        {
            // Move towards takes a current position, a target position and a max distance delta.
            // The delta is how far the move should happen this frame. 
            // Using velocity * Time.deltaTime calculates the distance based on velocity over time.
            transform.position = Vector3.MoveTowards(transform.position, startingPosition, enemyVelocityReturn * Time.deltaTime);

            // Yield until the next frame
            yield return null;
        }
    }
}
