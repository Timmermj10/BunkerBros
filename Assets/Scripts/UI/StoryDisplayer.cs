using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StoryDisplayer : MonoBehaviour
{

    private TMP_Text text;
    private string fullText = "In a post apocalyptic world, our heroes have been chased by zombies into the desert where they are stranded until they find a way to escape. They have taken shelter in a government bunker, equipped with remote controlled robots and drones to help them defend themselves. They must now survive long enough to execute an escape plan.";
    private float delayBetweenCharacters = 0.03f;
    private float fadeOutDuration = 2f;

    public AudioClip sound1;
    public AudioClip sound2;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private bool storyDone = false;

    private bool player1Confirmed = false;
    private bool player2Confirmed = false;

    public GameObject managerConfirmation;
    public GameObject playerConfirmation;

    public GameObject managerPrompt;
    public GameObject playerPrompt;

    private void Start()
    {
        text = GetComponent<TMP_Text>();

        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource1.clip = sound1;
        audioSource2.clip = sound2;

        audioSource1.volume = 0.7f;

        text.text = string.Empty;
        StartCoroutine(TypeWords());
    }

    private IEnumerator TypeWords()
    {

        text.text = string.Empty;
        foreach (char letter in fullText.ToCharArray())
        {
            text.text += letter;
            yield return new WaitForSeconds(delayBetweenCharacters);

            if (audioSource1 && !audioSource1.isPlaying)
            {
                audioSource1.Play();
            }
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(TextFadeOutCoroutine(text));
    }

    private IEnumerator TextFadeOutCoroutine(TMP_Text textComponent)
    {
        Color originalColor = textComponent.color;
        float timer = 0;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Main");
        asyncLoad.allowSceneActivation = false;

        if (audioSource2 && !audioSource2.isPlaying)
        {
            audioSource2.Play();
        }

        while (timer < fadeOutDuration)
        {
            // Calculate the blend factor proportionally to the time.
            float blend = Mathf.Clamp01(timer / fadeOutDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - blend);

            // Increment the timer by the time since last frame.
            timer += Time.deltaTime;

            yield return null; // Wait until next frame.
        }

        // Ensure text is fully transparent after the loop.
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        storyDone = true;

        playerPrompt.SetActive(true);
        managerPrompt.SetActive(true);

        Debug.Log("Waiting for player confirmation");
        while (!player1Confirmed || !player2Confirmed)
        {
            yield return null;
        }

        StartCoroutine(TextFadeOut(managerPrompt.GetComponent<TMP_Text>()));
        StartCoroutine(TextFadeOut(playerPrompt.GetComponent<TMP_Text>()));
        StartCoroutine(ImageFadeOut(playerConfirmation));
        StartCoroutine(ImageFadeOut(managerConfirmation));

        yield return new WaitForSeconds(2);

        asyncLoad.allowSceneActivation = true;

    }

    private IEnumerator TextFadeOut(TMP_Text textComponent)
    {
        Color originalColor = textComponent.color;
        float timer = 0;

        while (timer < fadeOutDuration)
        {
            // Calculate the blend factor proportionally to the time.
            float blend = Mathf.Clamp01(timer / fadeOutDuration);
            textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - blend);

            // Increment the timer by the time since last frame.
            timer += Time.deltaTime;

            yield return null; // Wait until next frame.
        }

        // Ensure text is fully transparent after the loop.
        textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
        yield return null;
    }

    private IEnumerator ImageFadeOut(GameObject image)
    {

        Image imageToFade = image.GetComponent<Image>();
        Color originalColor = imageToFade.color;
        float timer = 0;

        while (timer <= fadeOutDuration)
        {
            // Calculate alpha value proportionally to the elapsed time
            float alpha = Mathf.Lerp(1f, 0f, timer / fadeOutDuration);

            // Set the new color with the new alpha value
            imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            // Increment the timer by the time since last frame
            timer += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the image is fully transparent after the loop
        imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

        // Optionally deactivate the image game object after fading out
        imageToFade.gameObject.SetActive(false);
    }


    public void player1Confirm()
    {
        if (!player1Confirmed && storyDone)
        {
            player1Confirmed = true;
            playerConfirmation.SetActive(true);
            Debug.Log("Player 1 has confirmed.");
        }
    }

    public void player2Confirm()
    {
        if (!player2Confirmed && storyDone)
        {
            player2Confirmed = true;
            managerConfirmation.SetActive(true);
            Debug.Log("Player 2 has confirmed.");
        }
    }
}
