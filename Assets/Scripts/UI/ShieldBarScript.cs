using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldBarScript : MonoBehaviour
{

    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxShield(int health)
    {
        // Set the slider max value
        slider.maxValue = health;

        // Set the slider value
        slider.value = health;

        // Set the fill color
        fill.color = gradient.Evaluate(1f);
        fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, .5f);
    }

    public void SetShield(int health)
    {
        // Update the slider value
        slider.value = health;

        // Update the fill color
        fill.color = gradient.Evaluate(slider.normalizedValue);
        fill.color = new Color(fill.color.r, fill.color.g, fill.color.b, .5f);
    }
}
