using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KnifeAttack : MonoBehaviour
{
    public Animator anim;

    Subscription<AttackEvent> sub = null;

    void Awake()
    {
        sub = EventBus.Subscribe<AttackEvent>(_Attack);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(sub);
        sub = null;
    }
    void _Attack(AttackEvent e)
    {
        anim.SetTrigger("knife");
    }
}
