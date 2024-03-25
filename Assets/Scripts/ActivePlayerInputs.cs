using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivePlayerInputs : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float lookSpeed = 5f;
    private Vector2 movementInputValue;
    private Vector2 aimInputValue;
    private Rigidbody rb;

    private bool playerControls = false;

    //private bool playerControls = false;
    //private float shootingCooldown = 0.3f;
    //private float shootingTimer = 0f;
    private void Update()
    {
        transform.Rotate(Vector3.up, aimInputValue.x * lookSpeed);
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        EventBus.Subscribe<WaveStartedEvent>(WaveStarted);
        EventBus.Subscribe<WaveEndedEvent>(WaveEnded);

        //Debug.Log("Turning Off Player Controls");
        playerControls = false;
}


    private void WaveStarted(WaveStartedEvent e)
    {
        playerControls = true;
        //Debug.Log("Turning on Player Controls");
    }

    private void WaveEnded(WaveEndedEvent e)
    {
        playerControls = false;
        Debug.Log("Turning Off Player Controls");
    }


    // Constantly sets the value of movementInputValue to the current input on the left joystick
    private void OnMove(InputValue value)
    {
        if (playerControls)
        {
            //Debug.Log("Moving the player");
            movementInputValue = value.Get<Vector2>();
            Vector3 forward = movementInputValue.y * transform.forward;
            Vector3 right = movementInputValue.x * transform.right;
            rb.velocity = moveSpeed * (forward + right);
            //Debug.Log("Active Player: MovementInputValue = " + movementInputValue);

            if (movementInputValue == Vector2.zero)
            {
                rb.velocity = Vector3.zero;
            }

        } else
        {
            rb.velocity = Vector3.zero;
        }
    }

    // Constantly sets the value of aimInputValue to the current input on the right joystick
    private void OnAim(InputValue value)
    {
        if (playerControls)
        {
            aimInputValue = value.Get<Vector2>();
            //Debug.Log("Active Player: AimInputValue = " + aimInputValue);
        }
    }


    public Vector2 getAimValue() { return aimInputValue; }

    public Vector2 getMovementValue() { return movementInputValue; }


    // Rounds any vector 2 to the nearest 16th of a cardinal direction (N, NNE, NE, ENE, E, etc)
    public static Vector2 RoundVectorToDirection(Vector2 vector)
    {
        if (vector == Vector2.zero)
        {
            return vector;
        }
        float angle = Mathf.Atan2(vector.y, vector.x) * Mathf.Rad2Deg;
        float roundedAngle = Mathf.Round(angle / 22.5f) * 22.5f;
        float roundedRadians = roundedAngle * Mathf.Deg2Rad;

        Vector2 roundedVector = new Vector2(Mathf.Cos(roundedRadians), Mathf.Sin(roundedRadians));
        return roundedVector.normalized;
    }

    private void OnAttack(InputValue value)
    {
        if (playerControls)
        {
            //Debug.Log("Active Player: Player Attacked");
            EventBus.Publish(new AttackEvent());
        }
    }
}
