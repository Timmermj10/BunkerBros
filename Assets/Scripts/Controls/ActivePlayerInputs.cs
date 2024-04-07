using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class ActivePlayerInputs : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 lookSpeed = new Vector2(90, 90);
    public float pingDistance = 100f;
    public LayerMask ignore;

    private Vector2 movementInputValue;
    private Vector2 aimInputValue;
    private Vector2 rotation;
    private Rigidbody rb;
    private Transform look;
    private bool playerControls = true;
    private PingManager pingManager;

    private void Awake()
    {
        EventBus.Subscribe<WaveStartedEvent>(WaveStarted);
        rb = GetComponent<Rigidbody>();
        look = transform.Find("PlayerLook");
        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();
    }

    private void FixedUpdate() {
        Vector3 forward = movementInputValue.y * transform.forward;
        Vector3 right = movementInputValue.x * transform.right;
        rb.velocity = rb.velocity.y * Vector3.up + moveSpeed * (forward + right);
        if (playerControls)
        {
            rotation += Time.deltaTime * new Vector2(-aimInputValue.y * lookSpeed.y, aimInputValue.x * lookSpeed.x);
            rotation.x = Mathf.Clamp(rotation.x, -89, 89);
            look.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
    }


    private void WaveStarted(WaveStartedEvent e)
    {
        //Debug.Log("Turning on Player Controls");
        playerControls = true;
    }

    //private void WaveEnded(WaveEndedEvent e)
    //{
    //    playerControls = false;
    //    //Debug.Log("Turning Off Player Controls");
    //}


    // Constantly sets the value of movementInputValue to the current input on the left joystick
    private void OnMove(InputValue value)
    {
        movementInputValue = value.Get<Vector2>();
    }

    // Constantly sets the value of aimInputValue to the current input on the right joystick
    private void OnAim(InputValue value)
    {
        aimInputValue = value.Get<Vector2>();
    }

    private void OnAttack(InputValue value)
    {
        if (playerControls)
        {
            //Debug.Log("Active Player: Player Attacked");
            EventBus.Publish(new AttackEvent());
        }
    }

    private void OnPing(InputValue value)
    {
        RaycastHit hit;
        if(Physics.Raycast(look.position, look.forward, out hit, 100f, ~ignore))
        {
            HasPing hasPing = hit.transform.gameObject.GetComponent<HasPing>();
            if (hasPing)
                hasPing.TogglePing();
            else
                pingManager.PlayerPing(hit.point);
        }
        else
        {
            Debug.Log("ping missed");
        }
    }

    private void OnADS(InputValue value)
    {
        Debug.Log("ADS");
    }

    private void OnJump(InputValue value)
    {
        Debug.Log("jump");
    }

    public Vector2 getAimValue() { return aimInputValue; }

    public Vector2 getMovementValue() { return movementInputValue; }

    
}
