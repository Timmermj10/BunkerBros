using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TooltipPositioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPanel;
    public float xOffset = 10f;  // Horizontal offset from the button

    // String fields to input the cost and descriptions for the item tooltips
    public string cost;
    public string description;

    // Assumes tooltipPanel starts off as inactive
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (gameObject.GetComponent<Button>().enabled)
        {
            // Make sure tooltip panel is active
            tooltipPanel.SetActive(true);

            // Set the tooltip panel to the right of the button that was hovered.
            Vector3 buttonPos = transform.position;
            float buttonWidth = GetComponent<RectTransform>().sizeDelta.x;

            // Calculate the new position
            Vector3 tooltipPos = new Vector3(buttonPos.x + buttonWidth / 2 + xOffset, buttonPos.y - 65, buttonPos.z);

            // Set the tooltip panel's position relative to the button
            tooltipPanel.transform.position = tooltipPos;

            // Update the tooltip text
            Text[] fields = tooltipPanel.GetComponentsInChildren<Text>();
            fields[0].text = cost;
            fields[1].text = description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Deactivate the tooltip panel when the cursor is no longer over the button
        tooltipPanel.SetActive(false);
    }

    void Start()
    {
        // Hide tooltip panel initially if it's not hidden already
        if (tooltipPanel.activeSelf)
        {
            tooltipPanel.SetActive(false);
        }
    }
}
