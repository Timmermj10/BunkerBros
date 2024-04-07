using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingManager : MonoBehaviour
{
    public GameObject soloPing;
    public GameObject warnPing;
    public float managerBuffer = .2f;

    Transform managerCam;
    Transform playerCam;
    List<Transform> pings = new();
    GameObject playerPing;
    GameObject managerPing;
    HasPing playerHasPing;
    HasPing managerHasPing;

    void Awake()
    {
        playerCam = GameObject.FindGameObjectWithTag("PlayerCam").transform;
        managerCam = GameObject.FindGameObjectWithTag("ManagerCam").transform;
        foreach (GameObject ping in GameObject.FindGameObjectsWithTag("Ping"))
        {
            pings.Add(ping.transform);
        }
        playerPing = Instantiate(soloPing);
        managerPing = Instantiate(soloPing);
        playerHasPing = playerPing.GetComponentInChildren<HasPing>();
        managerHasPing = managerPing.GetComponentInChildren<HasPing>();
        pings.Add(playerPing.transform.Find("spotted"));
        pings.Add(managerPing.transform.Find("spotted"));
    }

    void Update()
    {
        foreach (Transform ping in pings)
        {
            Transform playerView = ping.Find("playerView");
            playerView.LookAt(playerCam.position);
            Transform managerView = ping.Find("managerView");
            float vw = 5-managerBuffer, vh = 5-managerBuffer;
            Vector3 offset = ping.position - managerCam.position;
            offset.y = 0;
            Vector3 clamped = new(Mathf.Clamp(offset.x, -vw, vw), offset.y, Mathf.Clamp(offset.z, -vh, vh));
            if (clamped == offset)
            {
                managerView.Find("direction").gameObject.SetActive(false);
                managerView.localPosition = Vector3.zero;
            }
            else{
                Transform direction = managerView.Find("direction");
                direction.gameObject.SetActive(true);
                direction.localRotation = Quaternion.Euler(new(0f, 0f, Mathf.Atan2(offset.x, -offset.z)*Mathf.Rad2Deg));
                managerView.position = managerCam.position + clamped;
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

    public IEnumerator Warn(Vector3 pos, float time)
    {
        GameObject warning = Instantiate(warnPing, pos, Quaternion.identity);
        pings.Add(warning.transform);
        yield return new WaitForSeconds(time);
        pings.Remove(warning.transform);
        Destroy(warning);
    }
}
