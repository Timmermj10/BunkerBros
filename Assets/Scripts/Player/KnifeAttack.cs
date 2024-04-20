using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KnifeAttack : MonoBehaviour
{
    public Animator anim;

    private Subscription<AttackEvent> sub;
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
        anim.SetTrigger("knife");
        EventBus.Publish(new KnifeAttackSoundEvent());
        Debug.Log("Published KnifeAttackSoundEvent");
    }
}
