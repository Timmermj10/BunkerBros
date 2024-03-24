using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{

    public BoxCollider detecter;
    public float speed = 5f;
    private GameObject obj;
    private GameObject player;
    private Vector3 obj_loca;
    private Vector3 player_loca;
    private bool attack = false;
    private bool isChasingPlayer = false;


    void Awake()
    {
        obj = GameObject.Find("Objective");
        obj_loca = obj.transform.position;
        player = GameObject.Find("player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!attack)
        {
            if (!isChasingPlayer)
            {
                move(obj.transform.position);
            }
            else if (player != null)
            {
                move(player.transform.position);
            }
        }
    }

    private void move(Vector3 location)
    {
        Vector3 play = location - transform.position;
        Vector3 newLoca = play.normalized * speed * Time.deltaTime;
        newLoca.y = 0;
       
        transform.LookAt(transform.position + newLoca);
        transform.position += newLoca;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.gameObject;
            isChasingPlayer = true;
            
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            player = null;
            isChasingPlayer = false;
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Objective")
        {
            attack = true;
        } 
    }

    

}


