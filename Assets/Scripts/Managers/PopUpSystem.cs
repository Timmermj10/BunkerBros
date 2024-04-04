using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public Animator animator;
    public TMP_Text popUpText;

    private float delayBetweenCharacters = 0.03f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        popUpText.text = "";
    }

    public void popUp(string playerType, string text)
    {
        RectTransform rectTransform = popUpBox.GetComponent<RectTransform>();
        if (playerType == "Manager")
        {
            rectTransform.anchoredPosition = new Vector2(-480, 0);
        }
        else if (playerType == "Player")
        {
            rectTransform.anchoredPosition = new Vector2(480, 0);
        }

        popUpBox.SetActive(true);
        popUpText.text = "";

        StartCoroutine(TypeWords(playerType, text));
    }

    private IEnumerator TypeWords(string playerType, string fullText)
    {

        popUpText.text = string.Empty;
        foreach (char letter in fullText.ToCharArray())
        {
            popUpText.text += letter;
            yield return new WaitForSeconds(delayBetweenCharacters);

            if (audioSource && !audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }

        EventBus.Publish(new PopUpEndEvent(playerType));
        yield return new WaitForSeconds(0.5f);

        while (!Input.anyKey)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpBox.SetActive(false);
        popUpText.text = "";
    }
}
