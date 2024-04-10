using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class ActivePlayerInputs : MonoBehaviour
{
    public float crouchSpeed = 2.5f;
    public float walkSpeed = 5f;
    public float runSpeed = 7.5f;
    public float jumpHeight = 5f;
    public float slideTime = 1f;
    public Vector2 lookSpeed = new Vector2(90, 90);
    public float pingDistance = 100f;
    public LayerMask ignore;
    public LayerMask ground;

    private Vector2 movementInputValue;
    private Vector2 aimInputValue;
    private Vector2 rotation;
    private Rigidbody rb;
    private Transform look;
    private bool playerControls = true;
    private PingManager pingManager;
    private Animator anim;

    bool willCrouch = false;
    private void Awake()
    {
        EventBus.Subscribe<WaveStartedEvent>(WaveStarted);
        rb = GetComponent<Rigidbody>();
        look = transform.Find("PlayerLook");
        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        bool running = anim.GetBool("running");
        bool crouched = anim.GetBool("crouching");
        bool ads = anim.GetBool("ads");
        bool knife = anim.GetBool("knife");
        /*Debug.Log(anim.GetCurrentAnimatorClipInfo(0)[0].clip.name + "\n" +
            "running: " + running + "\t" +
            "crouched: " + crouched + "\t" +
            "ads: " + ads + "\t" +
            "knife: " + knife);*/
        Vector3 forward = movementInputValue.y * transform.forward;
        Vector3 right = movementInputValue.x * transform.right;
        rb.velocity = rb.velocity.y * Vector3.up + (running ? runSpeed : (crouched ? crouchSpeed : walkSpeed)) * (forward + right);
        if (playerControls)
        {
            rotation += Time.deltaTime * new Vector2(-aimInputValue.y * lookSpeed.y, aimInputValue.x * lookSpeed.x);
            rotation.x = Mathf.Clamp(rotation.x, -89, 89);
            look.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
        if (Grounded() && !crouched && willCrouch)
            if(running)
            {
                Slide();
            }
            else
            {
                running = false;
                Crouch();
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
        if (movementInputValue == Vector2.zero)
            anim.SetBool("running", false);
    }
    private void OnRun(InputValue value)
    {
        if (Grounded())
            anim.SetBool("running", true);

    }
    // Constantly sets the value of aimInputValue to the current input on the right joystick
    private void OnLook(InputValue value)
    {
        aimInputValue = value.Get<Vector2>();
    }
    private void OnAim(InputValue value)
    {
        anim.SetBool("ads", value.Get<float>() > 0);
    }
    private void OnAttack(InputValue value)
    {
        if (playerControls)
        {
            //Debug.Log("Active Player: Player Attacked");
            EventBus.Publish(new AttackEvent());
        }
    }
    private void OnJump(InputValue value)
    {
        if (Grounded())
            rb.velocity = new(rb.velocity.x, jumpHeight, rb.velocity.y);
    }
    private void OnCrouch(InputValue value)
    {
        bool crouched = anim.GetBool("crouching");
        if (crouched) anim.SetBool("crouching", false);
        else willCrouch = true;
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

    void OnInteract(InputValue value)
    {
        PlayerInteract.buttonPressed = value.isPressed;
    }

    public Vector2 getAimValue() { return aimInputValue; }

    public Vector2 getMovementValue() { return movementInputValue; }

    bool Grounded()
    {
        CapsuleCollider coll = GetComponentInChildren<CapsuleCollider>();
        return Physics.Raycast(transform.position, Vector3.down, coll.bounds.extents.y + .05f, ground);
    }
    private void Crouch()
    {
        willCrouch = false;
        anim.SetBool("crouching", true);
    }
    private void Stand()
    {
        anim.SetBool("crouching", false);
    }
    public IEnumerator Slide()
    {
        Crouch();
        yield return new WaitForSeconds(1f);
        Stand();
    }
}
