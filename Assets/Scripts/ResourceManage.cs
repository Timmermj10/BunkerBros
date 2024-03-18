using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManage : MonoBehaviour
{
    public float timer = 30f;
    public int value = 10;
    private float time;

    private void Awake()
    {
        time = Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Detected collision between {gameObject.name} and {other.gameObject.name}");

        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log($"Coin collected by {other.gameObject.name} with tag {other.gameObject.tag}");
            Destroy(gameObject);
            EventBus.Publish<CoinCollect>(new CoinCollect(value));
        }
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            Destroy(gameObject);
            EventBus.Publish<CoinCollect>(new CoinCollect(value / 2));
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