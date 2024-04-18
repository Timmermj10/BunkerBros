using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class TIleHoverUI : MonoBehaviour
{
    // The preview prefab
    public GameObject previewPrefab;

    // For respawns
    public GameObject validPrefab;
    public GameObject invalidPrefab;

    // Preview gameobject instance
    private GameObject previewInstance;

    private void Start()
    {
        EventBus.Subscribe<ItemUseEvent>(_DespawnHover);
    }

    void Update()
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray mouseRay = GameObject.Find("ManagerCamera").GetComponent<Camera>().ScreenPointToRay(screenPosition);

        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, LayerMask.GetMask("Default", "Map", "Enemy", "Structure")) && (hit.collider.gameObject.name == "Cube" || hit.collider.gameObject.layer == 11 || ((hit.collider.gameObject.name == "Boulder") && (gameObject.name is "Nuke" || gameObject.name is "Missle"))))
        {
            // Now worldPosition contains the 3D point in world space where the mouse is pointing
            Vector3 worldPosition = hit.point;
            Vector3 worldPositionRounded = new Vector3(Mathf.RoundToInt(worldPosition.x), 15, Mathf.RoundToInt(worldPosition.z));

            // EventSystem.current holds a reference to the current event system
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

            // If we don't have an object selected, grab it from the most recent item
            if (selectedObj == null)
            {
                selectedObj = ManagerPlayerInputsNew.mostRecentItem;
            }

            if (selectedObj == gameObject && ManagerPlayerInputsNew.withinView(worldPositionRounded) && hit.transform.gameObject.layer != LayerMask.NameToLayer("Map") && hit.transform.gameObject.layer != LayerMask.NameToLayer("Structure"))
            {
                if (previewInstance == null)
                {
                    // Instantiate the preview instance
                    previewInstance = Instantiate(previewPrefab, worldPositionRounded, Quaternion.identity);
                }
                else
                {
                    // Update the preview instance if we are working with player respawn
                    if (selectedObj == GameObject.Find("PlayerRespawn"))
                    {
                        if (Vector3.Distance(worldPositionRounded, new Vector3(0, 15, 0)) < ManagerPlayerInputsNew.maxRespawnDistanceFromObjective && GameObject.Find("Player") == null)
                        {
                            if (previewInstance != GameObject.Find("ValidSpawnPreview(Clone)"))
                            {
                                Destroy(previewInstance);
                                previewInstance = Instantiate(validPrefab, worldPositionRounded, Quaternion.identity);
                            }
                        }
                        else
                        {
                            if (previewInstance != GameObject.Find("InvalidSpawnPreview(Clone)"))
                            {
                                Destroy(previewInstance);
                                previewInstance = Instantiate(invalidPrefab, worldPositionRounded, Quaternion.identity);
                            }
                        }
                    }

                    // Move the preview instance to the tile position
                    previewInstance.transform.position = worldPositionRounded;
                }
            }
            else
            {
                if (previewInstance != null)
                {
                    // Destroy or deactivate the preview instance when not hovering over a tile
                    Destroy(previewInstance);
                }
            }
        }
        // If we collide with something that isn't the prefab
        else if (previewInstance != null && (hit.collider == null || hit.collider.gameObject.name != previewInstance.name))
        {
            // Destroy or deactivate the preview instance when not hovering over a tile
            Destroy(previewInstance);
        }
    }

    public void _DespawnHover(ItemUseEvent e)
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
        }
    }
}
