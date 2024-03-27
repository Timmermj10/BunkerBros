using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class KnifeAttack : MonoBehaviour
{
    public float duration = .2f;
    private Transform hand;
    private float radius;
    private bool swinging = false;

    Subscription<AttackEvent> sub = null;
    Collider coll;

    void Awake()
    {
        hand = transform.parent;
        radius = hand.localPosition.magnitude;
        coll = GetComponent<Collider>();
        sub = EventBus.Subscribe<AttackEvent>(_Attack);
    }
    private void OnEnable()
    {
        if (sub == null) EventBus.Subscribe<AttackEvent>(_Attack);
    }
    private void OnDisable()
    {
        EventBus.Unsubscribe(sub);
        sub = null;
    }
    void _Attack(AttackEvent e)
    {
        if(!swinging)
            StartCoroutine(Swing());
    }
    IEnumerator Swing()
    {
        swinging = true;
        coll.enabled = true;
        float startAngle = Mathf.Atan2(hand.localPosition.x, hand.localPosition.z);
        float startTime = Time.time;
        while (Time.time - startTime < duration)
        {
            float newAngle = startAngle - Mathf.Sin((Time.time - startTime) / duration * Mathf.PI);
            hand.localPosition = radius * new Vector3(Mathf.Sin(newAngle), 0f, Mathf.Cos(newAngle));
            yield return null;
        }
        hand.localPosition = radius * new Vector3(Mathf.Sin(startAngle), 0f, Mathf.Cos(startAngle));
        coll.enabled = false;
        swinging = false;
    }
}