using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    private Text promptText;

    // Start is called before the first frame update
    void Start()
    {
        promptText = GameObject.Find("PromptText").GetComponent<Text>();
    }

    public void UpdateText(string promptMessage)
    {
        if (promptText != null) promptText.text = promptMessage;
    }
}
