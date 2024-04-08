using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class TetherFPV : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    private Camera cam;

    private int fullyRenderingCullingMask;
    private Color funnlyRenderingBackgroundColor;
    private CameraClearFlags fullyRenderingFlags;

    private bool dropping = false;


    private void Awake()
    {
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);
        EventBus.Subscribe<ObjectDestroyedEvent>(_CheckPlayerDeath);
        EventBus.Subscribe<AirDropStartedEvent>(_FollowPlayerRespawn);
        EventBus.Subscribe<AirdropLandedEvent>(_endDrop);

        cam = GetComponent<Camera>();
        fullyRenderingCullingMask = cam.cullingMask;
        funnlyRenderingBackgroundColor = cam.backgroundColor;
        fullyRenderingFlags = cam.clearFlags;

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.cullingMask = 0;
    }

    void LateUpdate()
    {
        if (target)
        {
            transform.position = target.position + offset;
            transform.rotation = target.rotation;
        }
    }

    private void _PlayerRespawn(PlayerRespawnEvent e)
    {
        target = GameObject.Find("PlayerLook").transform;
    }

    private void _CheckPlayerDeath(ObjectDestroyedEvent e)
    {
        if (e.name is "player")
        {
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.cullingMask = 0;
        }
    }

    private void _FollowPlayerRespawn(AirDropStartedEvent e)
    {
        if (e.itemID == 9)
        {
            cam.clearFlags = fullyRenderingFlags;
            cam.backgroundColor = funnlyRenderingBackgroundColor;
            cam.cullingMask = fullyRenderingCullingMask;
            transform.rotation = Quaternion.identity;

            StartCoroutine(dropWithAirdrop(e.airdropTransform));
        }
    }

    private void _endDrop(AirdropLandedEvent e)
    {
        dropping = false;
    }

    IEnumerator dropWithAirdrop(Transform airdropTransform)
    {
        dropping = true;
        while (dropping)
        {
            transform.position = airdropTransform.position;
            yield return null;
        }
        yield return null;
    }
}
