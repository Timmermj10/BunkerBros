using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttack : MonoBehaviour
{
    private AmmoSystem ammo;
    public ProjectileBehavior ProjectilePrefab;
    public Transform aim;

    private Subscription<AttackEvent> sub;
    private Animator anim;

    private void Start()
    {
        ammo = GameObject.Find("Ammo").GetComponent<AmmoSystem>();
        anim = this.GetComponent<Animator>();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(sub);
    }
    private void OnEnable()
    {
        sub = EventBus.Subscribe<AttackEvent>(_Attack);
    }
    void _Attack(AttackEvent e)
    {
        if (ammo.ammo_count > 0)
        {
            Debug.DrawRay(aim.position, aim.forward, Color.magenta);
            anim.SetTrigger("shoot");
            Instantiate(ProjectilePrefab.gameObject, aim.position, Quaternion.LookRotation(aim.forward));
            EventBus.Publish<ShootEvent>(new ShootEvent());
        }
    }
}
