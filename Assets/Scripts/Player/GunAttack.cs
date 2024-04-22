using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttack : MonoBehaviour
{
    public GameObject bullet;
    public Transform bulletSpawn;
    public GameObject shell;
    public Transform shellSpawn;
    public float shellSpeed = 1f;
    public int magCount = 15;
    public int magSize = 15;
    public int ammoCount = 60;
    public int reloadCount = 60;

    private Animator anim;
    private Vector3 lastPos;
    private Vector3 velocity;
    private Subscription<AttackEvent> attack;
    private Subscription<PickUpEvent> pickup;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        lastPos = transform.position;
        velocity = Vector3.zero;
    }
    private void FixedUpdate()
    {
        velocity = (transform.position - lastPos) / Time.fixedDeltaTime;
        lastPos = transform.position;
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(attack);
        EventBus.Unsubscribe(pickup);
    }
    private void OnEnable()
    {
        attack = EventBus.Subscribe<AttackEvent>(_Attack);
        pickup = EventBus.Subscribe<PickUpEvent>(_refill);
    }
    void _Attack(AttackEvent e)
    {
        if (magCount == 0)
        {
            if (ammoCount == 0)
            {
                EventBus.Publish(new EmptyAmmo());
                return;
            }
            else
            {
                anim.SetTrigger("reload");
                magCount = Mathf.Min(ammoCount, magSize);
                ammoCount -= magCount;
                return;
            }
        }
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name == "shoot") {
            magCount--;
            anim.SetTrigger("shoot");
            anim.SetInteger("ammo", magCount);

            EventBus.Publish(new ShootEvent());
            Instantiate(bullet, bulletSpawn.position, Quaternion.LookRotation(bulletSpawn.forward));
            Instantiate(shell, shellSpawn.position, Quaternion.LookRotation(bulletSpawn.up)).GetComponent<Rigidbody>().velocity = velocity + shellSpeed * shellSpawn.forward;
        }
    }
    private void _refill(PickUpEvent p)
    {
        if (p.pickedUpItem == ActivePlayerInventory.activePlayerItems.AmmoKit)
        {
            ammoCount += reloadCount;
        }
    }
}
