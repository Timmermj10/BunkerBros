using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldPickup : MonoBehaviour
{
    public float TimeToPickup = 3.0f;

    private bool ButtonPressed = false;

    public List<GameObject> PickUpItems = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnHoldToGet(InputValue value)
    {
        ButtonPressed = value.isPressed;
    }
}
