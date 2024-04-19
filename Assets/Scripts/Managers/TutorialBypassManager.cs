using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialBypassManager : MonoBehaviour
{
    public GameObject AmmoCrate;

    void Start()
    {
        StartCoroutine(BypassTutorial());

        // Subscribe to purchase events for ammo button
        EventBus.Subscribe<PurchaseEvent>(_gunUse);
    }


    private IEnumerator BypassTutorial()
    {
        yield return new WaitForSeconds(2);
        EventBus.Publish(new FirstTutorialWaveEvent());
        EventBus.Publish(new TutorialEndedEvent());
        EventBus.Publish(new WaveEndedEvent());
        yield return null;
    }

    public void _gunUse(PurchaseEvent e)
    {
        if (e.purchasedItem.itemId == 3)
        {
            AmmoCrate.SetActive(true);
        }
    }
}
