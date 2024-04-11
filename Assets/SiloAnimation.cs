using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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

            ChangeMaterial changeMaterialScript = e.status.gameObject.GetComponent<ChangeMaterial>();

            if (changeMaterialScript != null)
            {
                changeMaterialScript.ChangeKnobMaterial(e.status.gameObject, false);
            }
            else
            {
                Debug.LogWarning("ChangeMaterial script not found on item: " + e.status.gameObject.name);
            }

            // Make it so you can not use the same silo until it is unloaded
            // e.status.gameObject.transform.Find("ControlPanel").tag = "Interactable";
        }
    }
}
