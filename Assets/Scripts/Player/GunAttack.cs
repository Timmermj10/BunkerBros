using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttack : MonoBehaviour
{
    private AmmoSystem ammo;
    public GameObject bullet;
    public Transform bulletSpawn;
    public GameObject shell;
    public Transform shellSpawn;
    public float shellSpeed = 1f;

    private Subscription<AttackEvent> attack;
    private Animator anim;
    private Vector3 lastPos;
    private Vector3 velocity;

    private void Start()
    {
        ammo = GameObject.Find("Ammo").GetComponent<AmmoSystem>();
        anim = this.GetComponent<Animator>();
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
    }
    private void OnEnable()
    {
        attack = EventBus.Subscribe<AttackEvent>(_Attack);
    }
    void _Attack(AttackEvent e)
    {
        anim.SetInteger("ammo", ammo.ammo_count);
        anim.SetTrigger("shoot");
        Instantiate(bullet, bulletSpawn.position, Quaternion.LookRotation(bulletSpawn.forward));
        Instantiate(shell, shellSpawn.position, Quaternion.LookRotation(bulletSpawn.up)).GetComponent<Rigidbody>().velocity = velocity + shellSpeed * shellSpawn.forward;
        EventBus.Publish<ShootEvent>(new ShootEvent());
    }
}
