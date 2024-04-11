using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBypassManager : MonoBehaviour
{ 
    void Start()
    {
        StartCoroutine(BypassTutorial());
    }


    private IEnumerator BypassTutorial()
    {
        yield return new WaitForSeconds(2);
        EventBus.Publish(new FirstTutorialWaveEvent());
        EventBus.Publish(new TutorialEndedEvent());
        EventBus.Publish(new WaveEndedEvent());
        yield return null;
    }
}
