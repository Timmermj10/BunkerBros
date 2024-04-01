using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour
{
    // Function for button click
    public void onButtonSelection()
    {
        EventBus.Publish<ManagerButtonClickEvent>(new ManagerButtonClickEvent(gameObject.GetComponent<Button>()));
    }
}
