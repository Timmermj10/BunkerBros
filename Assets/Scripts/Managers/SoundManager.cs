using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip toPlay;
    float cooldown = 1f;
    void Update()
    {
        if (cooldown <= 0f)
        {
            cooldown = 1f;
            AudioSource.PlayClipAtPoint(toPlay, Vector3.zero);
        }
        cooldown -= Time.deltaTime;
    }
}
