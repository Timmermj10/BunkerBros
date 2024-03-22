using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxHealth(int health)
    {
        // Set the slider max value
        slider.maxValue = health;

        // Set the slider value
        slider.value = health;

        // Set the fill color
        fill.color = gradient.Evaluate(1f);
    }

    public void SetHealth(int health)
    {
        // Update the slider value
        slider.value = health;

        // Update the fill color
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
