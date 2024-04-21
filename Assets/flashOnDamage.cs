using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class flashOnDamage : MonoBehaviour
{
    private bool flashing = false;
    public Color[] colors;
    public GameObject icon;

    void Start()
    {
        EventBus.Subscribe<ObjectiveDamagedEvent>(flashIcon);
    }

    public void flashIcon(ObjectiveDamagedEvent e)
    {
        if (!flashing && gameObject.transform.parent.gameObject.activeSelf)
        {
            flashing = true;
            StartCoroutine(ChangeColorCoroutine());
        }
    }

    IEnumerator ChangeColorCoroutine()
    {
        Color originalColor = icon.GetComponent<Image>().color;
        foreach (Color color in colors)
        {
            icon.GetComponent<Image>().color = color;
            yield return new WaitForSeconds(0.25f);
        }

        // Reset the color to its original state
        icon.GetComponent<Image>().color = originalColor;

        flashing = false;
    }
}
