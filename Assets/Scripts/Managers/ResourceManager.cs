using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public float timer = 30f;
    public int value = 10;
    public GameObject playerView;
    public GameObject managerView;
    private Color orig;
    private Color black;
    private float time;

    private void Awake()
    {
        time = Time.deltaTime;
        orig = playerView.GetComponent<MeshRenderer>().material.color;
        black = new Color(0f, 0f, 0f);

    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"Detected collision between {gameObject.name} and {other.gameObject.name}");

        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log($"Coin collected by {other.gameObject.name} with tag {other.gameObject.tag}");
            Destroy(gameObject);
            EventBus.Publish<CoinCollect>(new CoinCollect(value));
        }
    }

    private void FixedUpdate()
    {
        if (timer <= 0)
        {
            Destroy(gameObject);
            //EventBus.Publish<CoinCollect>(new CoinCollect(value / 2));
        } else
        {
            timer -= 1;
            if (timer <= 200)
            {
                //blinking logic
                if (timer % 20 == 0)
                {
                    playerView.GetComponent<MeshRenderer>().material.color = Color.black;
                    managerView.GetComponent<MeshRenderer>().material.color = Color.black;
                }
                else
                {
                    playerView.GetComponent<MeshRenderer>().material.color = orig;
                    managerView.GetComponent<MeshRenderer>().material.color = orig;
                }
            }
        }
    }

}
