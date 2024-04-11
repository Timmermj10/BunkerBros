using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractTimer : MonoBehaviour
{

    private bool timerActive = false;
    private Slider slider;

    void Start()
    {

        slider = GetComponent<Slider>();
        gameObject.SetActive(false);
        EventBus.Subscribe<InteractTimerStartedEvent>(_StartTimer);
        EventBus.Subscribe<InteractTimerEndedEvent>(_EndTimer);

        EventBus.Subscribe<newItemInPickupRangeEvent>(_DisplayTimer);
        EventBus.Subscribe<itemRemovedFromPickupRangeEvent>(_UpdateTimerDisplay);
    }

    private void _DisplayTimer(newItemInPickupRangeEvent e)
    {
        //Debug.Log("Showing interact timer");
        gameObject.SetActive(true);
    }

    private void _UpdateTimerDisplay(itemRemovedFromPickupRangeEvent e)
    {
        //Debug.Log($"Hiding interact timer. Count = {e.numItemsInRange}");
        if (e.numItemsInRange <= 0) gameObject.SetActive(false);
    }

    private void _StartTimer(InteractTimerStartedEvent e)
    {
        gameObject.SetActive(true);
        timerActive = true;
        StartCoroutine(incrementTimer(e.duration));
    }

    private void _EndTimer(InteractTimerEndedEvent e)
    {
        timerActive = false;
        slider.value = 0f;
        //gameObject.SetActive(false);
    }

    IEnumerator incrementTimer(float duration)
    {

        float localTimer = 0;

        while (timerActive && localTimer <= duration)
        {
            //Increment timer
            localTimer += Time.deltaTime;

            //Adjust slider to match progress
            slider.value = Mathf.Min(1f, localTimer / duration);

            yield return null;
        }
        slider.value = 0f;
        //gameObject.SetActive(false);

        yield return null;
    }
}
