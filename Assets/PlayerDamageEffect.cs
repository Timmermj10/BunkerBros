using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDamageEffect : MonoBehaviour
{
    [Header("Damage Overlay")]
    private Image overlay; // DamageOverlay GameObject
    private float duration = 0.8f; // how long the image will stay
    public float fadeSpeed = 1.5f; // how quickly the red will fade
    private float durationTimer = 0; // timer to check against the duration


    void Start()
    {
        overlay = GameObject.Find("DamageOverlay").GetComponent<Image>();
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);

        EventBus.Subscribe<PlayerDamagedEvent>(_DamageIndicator);
    }

    private void _DamageIndicator(PlayerDamagedEvent e)
    {
        StartCoroutine(playerDamageOverlay());
    }

    IEnumerator playerDamageOverlay()
    {
        durationTimer = 0;
        overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);

        if (overlay != null)
        {
            while (overlay.color.a > 0.05)
            {
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    // Fade the image
                    float tempAlpha = overlay.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
                }
                yield return new WaitForFixedUpdate();
            }
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }
    }
}
