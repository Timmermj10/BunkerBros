using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetBombManager : MonoBehaviour
{
    public GameObject carpetBombPlane;

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

        int offset = 0;

        for (int z = -50; z <= 50; z += 3)
        {
            
            switch (z % 9)
            {
                case -8:
                    offset = 0;
                    break;
                case -5:
                    offset = -5;
                    break;
                case -2:
                    offset = -10;
                    break;
                case 1:
                    offset = 0;
                    break;
                case 4:
                    offset = -5;
                    break;
                case 7:
                    offset = -10;
                    break;
                default:
                    break;
            }
            Debug.Log($"Spawning plane at {new Vector3(-50 + offset, 15, z)} z % 9 = {z % 9}");

            Instantiate(carpetBombPlane, new Vector3(-50 + offset, 15, z), Quaternion.Euler(new Vector3(180, 180, 90)));
        }


        for (int i = -50; i <= 51; i += 3)
        {
            for (int j = -50; j <= 51; j += 3)
            {
                nukeLocation = new Vector3(i, 30, j);

                switch (j % 9)
                {
                    case -8:
                        offset = 0;
                        break;
                    case -5:
                        offset = -5;
                        break;
                    case -2:
                        offset = -10;
                        break;
                    case 1:
                        offset = 0;
                        break;
                    case 4:
                        offset = -5;
                        break;
                    case 7:
                        offset = -10;
                        break;
                    default:
                        break;
                }
                nukeLocation.x = nukeLocation.x + offset;

                if (Physics.Raycast(nukeLocation, Vector3.down, out hit, Mathf.Infinity, ~LayerMask.GetMask("Enemy", "Player", "Pickup")) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default"))
                {
                    nukeLocation.y = hit.point.y + 0.5f;
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(4, nukeLocation, true)); //id is 4 for nuke
                }
            }
            yield return new WaitForSeconds(0.225f);
        }

        yield return new WaitForSeconds(1);

        EventBus.Publish(new GameOverEvent());

        yield return null;
    }
}
