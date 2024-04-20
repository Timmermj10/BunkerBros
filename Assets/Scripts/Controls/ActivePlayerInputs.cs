using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using static UnityEngine.Rendering.DebugUI;

public class ActivePlayerInputs : MonoBehaviour
{
    public float walkSpeed = 5f;
    public float runSpeed = 7.5f;
    public float jumpHeight = 5f;
    public Vector2 lookSpeed = new Vector2(90, 90);
    public float adsLookSpeed = .5f;
    public float pingDistance = 100f;
    public float slideSpeed = 2f;
    public LayerMask ignore;
    public LayerMask ground;

    private Vector2 movementInputValue;
    private Vector2 aimInputValue;
    private Vector3 velocity;
    private Vector2 rotation;
    private CharacterController controller;
    private Vector3 hitNormal;
    private Transform look;
    private bool playerControls = true;
    private PingManager pingManager;
    private Animator anim;
    private bool toJump = false;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        look = transform.Find("PlayerLook");
        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate() {
        bool running = anim.GetBool("running");
        bool ads = anim.GetBool("ads");
        bool knife = anim.GetBool("knife");
        Vector3 forward = movementInputValue.y * transform.forward;
        Vector3 right = movementInputValue.x * transform.right;

        EventBus.Publish(new PlayerMovingEvent(movementInputValue, running));

        if (playerControls)
        {
            velocity = velocity.y * Vector3.up + (running ? runSpeed : walkSpeed) * (forward + right);
            if (controller.isGrounded && Sliding())
            {
                //Debug.Log("sliding");
                velocity.x = hitNormal.x * slideSpeed;
                velocity.z = hitNormal.z * slideSpeed;
            }
            if (toJump)
            {
                //Debug.Log("jumping");
                velocity.y = jumpHeight;
                toJump = false;
            }
            controller.Move(velocity * Time.fixedDeltaTime);
            velocity += Physics.gravity * Time.fixedDeltaTime;
            rotation += Time.deltaTime * (ads ? adsLookSpeed : 1f) * new Vector2(-aimInputValue.y * lookSpeed.y, aimInputValue.x * lookSpeed.x);
            rotation.x = Mathf.Clamp(rotation.x, -89, 89);
            look.rotation = Quaternion.Euler(rotation.x, rotation.y, 0);
            transform.rotation = Quaternion.Euler(0, rotation.y, 0);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //Debug.Log(hit.gameObject.name);
        hitNormal = hit.normal;
    }

    public void enableControls()
    {
        //Debug.Log("Turning on Player Controls");
        playerControls = true;
    }

    public void disableControls()
    {
        //Debug.Log("Turning off Player Controls");
        playerControls = false;
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
        if (controller.isGrounded && !Sliding())
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
        if (controller.isGrounded && !Sliding())
            toJump = true;
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
    private bool Sliding()
    {
        return Vector3.Angle(Vector3.up, hitNormal) > controller.slopeLimit;
    }

    public Vector2 getAimValue() { return aimInputValue; }

    public Vector2 getMovementValue() { return movementInputValue; }

}
