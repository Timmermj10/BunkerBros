using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetBombManager : MonoBehaviour
{
    void Start()
    {

        EventBus.Subscribe<LastWaveOverEvent>(_carpetBomb);
    }

    private void _carpetBomb(LastWaveOverEvent e)
    {
        StartCoroutine(CarpetBomb());
    }

    private IEnumerator CarpetBomb()
    {
        Vector3 nukeLocation = Vector3.zero;
        RaycastHit hit;

        for (int i = -50; i <= 51; i += 2)
        {
            for (int j = -50; j <= 51; j += 2)
            {
                nukeLocation = new Vector3(i, 30, j);

                if (Physics.Raycast(nukeLocation, Vector3.down, out hit, Mathf.Infinity, ~LayerMask.GetMask("Enemy", "Player", "Pickup")) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    nukeLocation.y = hit.point.y + 0.5f;
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(4, nukeLocation, true)); //id is 4 for nuke
                }
            }
            yield return new WaitForSeconds(0.3f);
        }

        yield return new WaitForSeconds(3);

        EventBus.Publish(new GameOverEvent());

        yield return null;
    }
}
