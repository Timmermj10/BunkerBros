using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{

    ParticleSystem ps;

    static ParticleManager instance;

    private void Start()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }


        ps = GetComponent<ParticleSystem>();
        ps.Stop();
    }

    public static void RequestDust(Vector3 position)
    {
        instance.transform.position = position;

        instance.ps.Play();
        instance.StartCoroutine(StopPS());

    }

    static IEnumerator StopPS()
    {

        yield return null;
        yield return null;
        yield return null;

        instance.ps.Stop();
    }

}
