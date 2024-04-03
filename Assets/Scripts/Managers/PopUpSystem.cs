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
    private float fadeOutDuration = 3f;

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
        else
        {
            rectTransform.anchoredPosition = new Vector2(480, 0);
        }

        popUpBox.SetActive(true);
        popUpText.text = "";

        StartCoroutine(TypeWords(text));
    }

    private IEnumerator TypeWords(string fullText)
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

        while (!Input.anyKey)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpBox.SetActive(false);
        popUpText.text = "";

    }

    private IEnumerator FadeOutCoroutine()
    {
        Color originalColor = popUpText.color;
        float timer = 0;

        while (timer < fadeOutDuration)
        {
            // Calculate the blend factor proportionally to the time.
            float blend = Mathf.Clamp01(timer / fadeOutDuration);
            popUpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - blend);

            // Increment the timer by the time since last frame.
            timer += Time.deltaTime;

            yield return null; // Wait until next frame.
        }

        // Ensure text is fully transparent after the loop.
        popUpText.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

    }
}
