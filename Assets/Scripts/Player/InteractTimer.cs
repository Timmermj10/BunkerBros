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
        EventBus.Subscribe<InteractTimerStartedEvent>(StartTimer);
        EventBus.Subscribe<InteractTimerEndedEvent>(EndTimer);
    }


    private void StartTimer(InteractTimerStartedEvent e)
    {
        gameObject.SetActive(true);
        timerActive = true;
        StartCoroutine(incrementTimer(e.duration));
    }

    private void EndTimer(InteractTimerEndedEvent e)
    {
        timerActive = false;
        gameObject.SetActive(false);
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
        gameObject.SetActive(false);

        yield return null;
    }
}
