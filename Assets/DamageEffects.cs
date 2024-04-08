using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{
    public GameObject bloodEffectPrefab;
    public GameObject sparkEffectPrefab;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<DamageEffectEvent>(startEffect);
    }

    public void startEffect(DamageEffectEvent e)
    {
        // If we want blood effect
        if (e.blood)
        {
            // Instantiate blood effect at the position of the other GameObject
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, e.zombie.transform.position, Quaternion.identity);
            bloodEffect.transform.SetParent(e.zombie.transform);
            bloodEffect.transform.localPosition = new Vector3(0, 1.25f, 0);

            StartCoroutine(DestroyEffectAfterDelay(bloodEffect, 0.5f));
        }
        else
        {
            // Instantiate spark effect at the position of the other GameObject
            GameObject sparkEffect = Instantiate(sparkEffectPrefab, e.zombie.transform.position, Quaternion.identity);
            sparkEffect.transform.SetParent(e.zombie.transform);
            sparkEffect.transform.localPosition = new Vector3(0, 1.25f, 0);

            StartCoroutine(DestroyEffectAfterDelay(sparkEffect, 0.5f));
        }
    }

    private IEnumerator DestroyEffectAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (effect != null)
        {
            Destroy(effect);
        }
    }
}
