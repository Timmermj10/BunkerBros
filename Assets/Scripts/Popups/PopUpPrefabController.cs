using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PopUpPrefabController : MonoBehaviour
{
    private TMP_Text popUpText;
    private AudioSource audioSource;
    private float delayBetweenCharacters = 0.03f;

    private GameObject managerConfirm;
    private GameObject playerConfirm;
    private GameObject confirmBorder;

    private bool hasConfirmed = false;

    void Start()
    {
        Canvas canvasParent = GameObject.Find("Canvas").GetComponent<Canvas>();
        if (canvasParent != null)
        {
            transform.SetParent(canvasParent.transform, false);
        }

        confirmBorder = transform.Find("PopUpBackground/GreenConfirmationBorder").gameObject;
        managerConfirm = confirmBorder.transform.Find("ManagerConfirm").gameObject;
        playerConfirm = confirmBorder.transform.Find("PlayerConfirm").gameObject;

        confirmBorder.SetActive(false);
        managerConfirm.SetActive(false);
        playerConfirm.SetActive(false);

        //Debug.Log("PopUpBoxCreated");
    }

    public void printText(string playerType, string text)
    {
        PlayerInput playerInputs = GetComponent<PlayerInput>();

        if (playerInputs != null && playerType is "Player")
        {
            if (Gamepad.current != null)
            {
                playerInputs.SwitchCurrentControlScheme("ControllerPlayer", Gamepad.current);
            }
            else
            {
                Debug.LogError("No gamepad connected for activePlayer.");
            }
        }

        if (playerInputs != null && playerType is "Manager")
        {
            if (playerInputs != null)
            {
                playerInputs.SwitchCurrentControlScheme("KBMPlayer", Keyboard.current, Mouse.current);
            }
        }

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

        confirmBorder.SetActive(true);
        if (playerType is "Manager")
        {
            managerConfirm.SetActive(true);
        }
        else if (playerType is "Player")
        {
            playerConfirm.SetActive(true);
        }

        while (!hasConfirmed)
        {
            yield return null;
        }


        EventBus.Publish(new PopUpEndEvent(playerType));

        Destroy(gameObject);
    }

    private void OnEnterConfirm()
    {
        //Debug.Log("OnEnterConfirm called");
        hasConfirmed = true;
    }

    private void OnGamepadConfirm()
    {
        //Debug.Log("OnGamepadConfirm called");
        hasConfirmed = true;
    }

}
