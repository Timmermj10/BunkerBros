using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryDisplayer : MonoBehaviour
{

    [SerializeField] private TMP_Text textComponent;
    [SerializeField] private string fullText = "In a post apocalyptic world, our heros have been chased by zombies into the desert where they are stranded until reinforcements arrive. They have taken shelter in a government bunker, equipped with remote controlled robots and drones to help them defend themselves. They must now survive long enough until they are able to be extracted.";
    [SerializeField] private float delayBetweenCharacters = 0.03f;
    [SerializeField] private float fadeOutDuration = 3f;

    public AudioClip sound1;
    public AudioClip sound2;

    private AudioSource audioSource1;
    private AudioSource audioSource2;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();

        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();

        audioSource1.clip = sound1;
        audioSource2.clip = sound2;

        audioSource1.volume = 0.7f;

        textComponent.text = string.Empty;
        StartCoroutine(TypeWords());
    }

    private IEnumerator TypeWords()
    {

        textComponent.text = string.Empty;
        foreach (char letter in fullText.ToCharArray())
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(delayBetweenCharacters);

            if (audioSource1 && !audioSource1.isPlaying)
            {
                audioSource1.Play();
            }
        }

        yield return new WaitForSeconds(2f);
        StartCoroutine(FadeOutCoroutine());
    }

    private IEnumerator FadeOutCoroutine()
    {
        Color originalColor = textComponent.color;
        float timer = 0;

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("DropsTest");
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

        while (!asyncLoad.isDone)
        {
            // Check if the load has finished
            if (asyncLoad.progress >= 0.9f)
            {
                textComponent.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1);
                textComponent.text = "press any key to continue";

                if (Input.anyKey) // Or a specific key or button
                {
                    // Activate the scene
                    asyncLoad.allowSceneActivation = true;
                }
            }

            yield return null;
        }

    }

}
