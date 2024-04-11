using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyAmmoSound : MonoBehaviour
{
    private Subscription<EmptyAmmo> empt;
    public AudioSource aud;

    // Start is called before the first frame update
    void Start()
    {
        //aud = GetComponent<AudioSource>();
        empt = EventBus.Subscribe<EmptyAmmo>(_PlayAudio);
    }

    private void _PlayAudio(EmptyAmmo e)
    {
        aud.Play();
    }
}
