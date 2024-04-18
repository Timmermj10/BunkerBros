using UnityEngine;

public class ChangeMaterial : MonoBehaviour
{
    public void ChangeKnobMaterial(GameObject parentItem, bool enable)
    {
        // Find the controlBox within the RadioTower prefab
        Transform ControlPanel = parentItem.transform.Find("ControlPanel");

        // Find the managerIndicator within the RadioTower prefab
        Transform Indicators = parentItem.transform.Find("RadioTowerIndicators");

        // Check if the controlBox was found
        if (ControlPanel != null)
        {
            // Find the knob child object within the controlBox
            Transform OnKnob = ControlPanel.Find("OnKnob");
            Transform OffKnob = ControlPanel.Find("OffKnob");

            // Check if the knob was found
            if (OnKnob != null || OffKnob != null)
            {
                // Get the Renderer component of the knob
                Renderer Onrenderer = OnKnob.GetComponent<Renderer>();
                Renderer Offrenderer = OffKnob.GetComponent<Renderer>();

                // Change the material of the knob
                if (enable == true) {
                    Onrenderer.material.color = new Color(0.0f, 0.937f, 0.0f);
                    Offrenderer.material.color = new Color(0.1098f, 0.0f, 0.0f);
                }
                else if (enable == false) {
                    Onrenderer.material.color = new Color(0.0f, 0.1098f, 0.0f);
                    Offrenderer.material.color = new Color(0.937f, 0.0f, 0.0f);
                }

            }
            else
            {
                Debug.LogWarning("Knob not found within the controlBox.");
            }
        }
        else
        {
            Debug.LogWarning("ControlBox not found within the RadioTower prefab.");
        }

        // Check if the indicator was found
        if (Indicators != null)
        {
            // Loop through each child of the parent GameObject
            foreach (Transform Indicator in Indicators)
            {
                // Get the Renderer component of the indicator
                Renderer IndicatorRenderer = Indicator.GetComponent<Renderer>();

                // Change the material of the knob
                if (enable == true)
                {
                    IndicatorRenderer.material.color = new Color(0.0f, 0.937f, 0.0f);
                }
                else if (enable == false)
                {
                    IndicatorRenderer.material.color = new Color(0.937f, 0.0f, 0.0f);
                }
            }
        }
        else
        {
            Debug.LogWarning("Indicators not found within the RadioTower prefab.");
        }
    }
}
