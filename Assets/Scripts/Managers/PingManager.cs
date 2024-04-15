using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingManager : MonoBehaviour
{
    public GameObject playerPingPrefab;
    public GameObject managerPingPrefab;
    public GameObject investigatePing;
    public GameObject warnPing;
    public GameObject enemyPing;
    public float managerBuffer = .2f;
    public float vw;
    public float vh;

    Transform managerCam;
    Transform playerCam;
    List<Transform> pings = new();
    GameObject playerPing;
    GameObject managerPing;
    HasPing playerHasPing;
    HasPing managerHasPing;

    private Camera managerCamera;

    void Awake()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam").transform;
        managerCam = GameObject.FindGameObjectWithTag("ManagerCam").transform;
        foreach (GameObject ping in GameObject.FindGameObjectsWithTag("Ping"))
        {
            pings.Add(ping.transform);
        }
        playerPing = Instantiate(playerPingPrefab);
        managerPing = Instantiate(managerPingPrefab);
        playerHasPing = playerPing.GetComponentInChildren<HasPing>();
        managerHasPing = managerPing.GetComponentInChildren<HasPing>();
        pings.Add(playerPing.transform.Find("spotted"));
        pings.Add(managerPing.transform.Find("spotted"));
        foreach (Transform ping in pings)
        {
            ping.Find("managerView").rotation = Quaternion.LookRotation(-Vector3.up, Vector3.forward);
            if(ping.name != "permanent")
                ping.gameObject.SetActive(false);
        }
        managerCamera = GameObject.FindGameObjectWithTag("ManagerCam").GetComponent<Camera>();
        EventBus.Subscribe<PlayerRespawnEvent>(_OnRespawn);
    }
    public void _OnRespawn(PlayerRespawnEvent e)
    {
        pings.Add(e.activePlayer.transform.Find("Ping").transform);
    }
    void Update()
    {
        foreach (Transform ping in pings)
        {
            if (ping)
            {
                Transform playerView = ping.Find("playerView");
                playerView.LookAt(playerView.position - playerCam.forward);
                Transform managerView = ping.Find("managerView");
                vw = managerCamera.orthographicSize - managerBuffer;
                vh = vw;
                Vector3 offset = ping.position - managerCam.position;
                Vector3 clamped = new(Mathf.Clamp(offset.x, -vw, vw), offset.y, Mathf.Clamp(offset.z, -vh, vh));
                if (clamped == offset)
                {
                    managerView.Find("direction").gameObject.SetActive(false);
                    managerView.localPosition = Vector3.zero;
                }
                else
                {
                    Transform direction = managerView.Find("direction");
                    direction.gameObject.SetActive(true);
                    direction.localRotation = Quaternion.Euler(new(0f, 0f, Mathf.Atan2(offset.x, -offset.z) * Mathf.Rad2Deg));
                    managerView.position = managerCam.position + clamped;
                }
            }
        }
    }

    public void PlayerPing(Vector3 location)
    {
        playerPing.transform.position = location;
        playerHasPing.Ping();
    }

    public void ManagerPing(Vector3 location)
    {
        managerPing.transform.position = location;
        managerHasPing.Ping();
    }

    private IEnumerator PingCoroutine(Vector3 pos, float time, PingType type = PingType.INVESTIGATE)
    {
        GameObject ping;
        if(type == PingType.INVESTIGATE) ping = Instantiate(investigatePing, pos, Quaternion.identity);
        else if(type == PingType.WARN) ping = Instantiate(warnPing, pos, Quaternion.identity);
        else ping = Instantiate(enemyPing, pos, Quaternion.identity);

        pings.Add(ping.transform);
        yield return new WaitForSeconds(time);
        pings.Remove(ping.transform);
        Destroy(ping);
    }

    public void Ping(Vector3 pos, float time, PingType type = PingType.INVESTIGATE)
    {
        StartCoroutine(PingCoroutine(pos, time, type));
    }
}

public enum PingType
{
    WARN,
    INVESTIGATE,
    ENEMY
}