using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ActivePlayerInputs : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 movementInputValue;
    private Vector2 aimInputValue;

    //private bool playerControls = false;
    //private float shootingCooldown = 0.3f;
    //private float shootingTimer = 0f;

    public ProjectileBehavior ProjectilePrefab;
    private void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * new Vector3(movementInputValue.x, 0, movementInputValue.y);
    }

    // Constantly sets the value of movementInputValue to the current input on the left joystick
    private void OnMove(InputValue value)
    {
        movementInputValue = value.Get<Vector2>();
        //Debug.Log("Active Player: MovementInputValue = " + movementInputValue);
    }

    // Constantly sets the value of aimInputValue to the current input on the right joystick
    private void OnAim(InputValue value)
    {
        if(value.Get<Vector2>() != Vector2.zero)
        {
            aimInputValue = value.Get<Vector2>();
            transform.LookAt(new Vector3(aimInputValue.x, transform.position.y, aimInputValue.y));
        }
        //Debug.Log("Active Player: AimInputValue = " + aimInputValue);
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
        Debug.Log("Active Player: Player Attacked");
        EventBus.Publish(new AttackEvent());
        

        
        if (aimInputValue != Vector2.zero)
        {
            //shootingTimer = shootingCooldown;
            Vector2 aimDirection = aimInputValue.normalized;
            Vector3 spawnPosition = gameObject.transform.position + new Vector3(aimDirection.x / 1.6f, 0f, aimDirection.y);

            float spawnAngle = Mathf.Atan2(-aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, spawnAngle, 0f);


            GameObject projectileObject = Instantiate(ProjectilePrefab.gameObject, spawnPosition, rotation);
            //ProjectileBehavior projectile = projectileObject.GetComponent<ProjectileBehavior>();
        }
        

    }
}
