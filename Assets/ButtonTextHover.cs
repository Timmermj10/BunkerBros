using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTextHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Text buttonText;
    public Color hoverColor;
    private Color originalColor;

    void Start()
    {
        // Grab reference to the button text
        buttonText = GetComponentInChildren<Text>();

        // Store the original color of the text
        originalColor = buttonText.color;
    }

    // Called when the pointer enters the button area
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the text color to hover color
        buttonText.color = hoverColor;
    }

    // Called when the pointer exits the button area
    public void OnPointerExit(PointerEventData eventData)
    {
        // Change the text color back to the original color
        buttonText.color = originalColor;
    }
}
