using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManage : MonoBehaviour
{
    public float timer = 30f;
    public int value = 2;
    private float time;

    private void Awake()
    {
        time = Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            EventBus.Publish<CoinCollect>(new CoinCollect(value));
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            EventBus.Publish<CoinCollect>(new CoinCollect(value / 2));
            Destroy(gameObject);
        } else
        {
            timer -= 1;
        }
    }

}

public class CoinCollect
{
    public int value;

    public CoinCollect(int _new_value) { value = _new_value; }
}