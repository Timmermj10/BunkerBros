using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiloAnimation : MonoBehaviour
{
    // Duration of the rotation
    public float duration = 1.0f;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<SiloLoadedEvent>(siloAnimationLoad);
        EventBus.Subscribe<SiloUnloadedEvent>(siloAnimationUnload);

        animator = GetComponent<Animator>();
    }

    public void siloAnimationLoad(SiloLoadedEvent e)
    {
        if (e.status.gameObject == gameObject)
        {
            animator.SetBool("Open", true);
        }
    }

    public void siloAnimationUnload(SiloUnloadedEvent e)
    {
        if (e.status.gameObject == gameObject)
        {
            animator.SetBool("Open", false);
        }
    }
}
