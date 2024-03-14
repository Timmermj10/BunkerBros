using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    
    public float speed = 5f;
    private GameObject obj;
    private Vector3 obj_loca;
    private bool detect = false;

    
    void Awake()
    {
        obj = GameObject.Find("Objective");
        obj_loca = obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.deltaTime;
        move();
    }

    private void move()
    {
        Vector3 play = obj_loca - transform.position;
        Vector3 newLoca = play.normalized * speed * Time.deltaTime;
        newLoca.y = 0;
        
        transform.position += newLoca;
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && (detect == false))
        {
            obj = other.gameObject;
            obj_loca = obj.transform.position;
            detect = true;
            Debug.Log("Detected");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            detect = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            obj = GameObject.Find("Objective");
            obj_loca = obj.transform.position;
            detect = false;
        }
    }

}


