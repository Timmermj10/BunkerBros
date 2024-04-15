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
    private int camFullyRenderingCullingMask;
    private Color camFunnlyRenderingBackgroundColor;
    private CameraClearFlags camFullyRenderingFlags;

    private Camera pings;
    private int pingsFullyRenderingCullingMask;
    private Color pingsFunnlyRenderingBackgroundColor;
    private CameraClearFlags pingsFullyRenderingFlags;

    private bool dropping = false;


    private void Awake()
    {
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);
        EventBus.Subscribe<ObjectDestroyedEvent>(_CheckPlayerDeath);
        EventBus.Subscribe<AirDropStartedEvent>(_FollowPlayerRespawn);
        EventBus.Subscribe<AirdropLandedEvent>(_endDrop);

        cam = GetComponent<Camera>();
        pings = cam.transform.Find("Pings").GetComponent<Camera>();

        camFullyRenderingCullingMask = cam.cullingMask;
        camFunnlyRenderingBackgroundColor = cam.backgroundColor;
        camFullyRenderingFlags = cam.clearFlags;

        pingsFullyRenderingCullingMask = pings.cullingMask;
        pingsFunnlyRenderingBackgroundColor = pings.backgroundColor;
        pingsFullyRenderingFlags = pings.clearFlags;

        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = Color.black;
        cam.cullingMask = 0;

        pings.clearFlags = CameraClearFlags.SolidColor;
        pings.backgroundColor = Color.black;
        pings.cullingMask = 0;
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

            pings.clearFlags = CameraClearFlags.SolidColor;
            pings.backgroundColor = Color.black;
            pings.cullingMask = 0;
        }
    }

    private void _FollowPlayerRespawn(AirDropStartedEvent e)
    {
        if (e.itemID == 9)
        {
            cam.clearFlags = camFullyRenderingFlags;
            cam.backgroundColor = camFunnlyRenderingBackgroundColor;
            cam.cullingMask = camFullyRenderingCullingMask;

            pings.clearFlags = pingsFullyRenderingFlags;
            pings.backgroundColor = pingsFunnlyRenderingBackgroundColor;
            pings.cullingMask = pingsFullyRenderingCullingMask;
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
