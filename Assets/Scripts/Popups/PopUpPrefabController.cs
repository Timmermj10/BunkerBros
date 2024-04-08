using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpPrefabController : MonoBehaviour
{
    private TMP_Text popUpText;
    private AudioSource audioSource;
    private float delayBetweenCharacters = 0.03f;

    void Start()
    {
        Canvas canvasParent = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvasParent != null)
        {
            transform.SetParent(canvasParent.transform, false);
        }
    }

    public void printText(string playerType, string text)
    {

        popUpText = GetComponentInChildren<TMP_Text>();
        RectTransform rectTransform = transform.GetComponent<RectTransform>();
        if (playerType == "Manager")
        {
            rectTransform.anchoredPosition = new Vector2(-480, 0);
        }
        else if (playerType == "Player")
        {
            rectTransform.anchoredPosition = new Vector2(480, 0);
        }


        if (popUpText == null)
        {
            Debug.Log("PopUpText is null");
        } else
        {
            popUpText.text = "";
            gameObject.SetActive(true);

            StartCoroutine(TypeWords(playerType, text));
        }
    }

    private IEnumerator TypeWords(string playerType, string fullText)
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("Audio source is null");
        }
        popUpText.text = string.Empty;

        //Debug.Log("Playing Sound");
        audioSource.Play();

        foreach (char letter in fullText.ToCharArray())
        {

            popUpText.text += letter;
            yield return new WaitForSeconds(delayBetweenCharacters);
        }

        //Debug.Log("Stopping Sound");
        audioSource.Stop();

        yield return new WaitForSeconds(1.2f);
        EventBus.Publish(new PopUpEndEvent(playerType));

        Destroy(gameObject);
    }
}
