using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffects : MonoBehaviour
{
    public GameObject bloodEffectPrefab;

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

            Debug.Log("here");
            // Instantiate blood effect at the position of the other GameObject
            GameObject bloodEffect = Instantiate(bloodEffectPrefab, e.zombie.transform.position, Quaternion.identity);
            bloodEffect.transform.SetParent(e.zombie.transform);
            bloodEffect.transform.localPosition = new Vector3(0, 1.5f, 0);

            StartCoroutine(DestroyEffectAfterDelay(bloodEffect, 1f));
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
