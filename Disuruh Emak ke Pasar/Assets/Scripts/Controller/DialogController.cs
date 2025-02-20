using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField]
    private Animator dialogPanel;
    [SerializeField]
    private TextMeshProUGUI dialogNameText;
    [SerializeField]
    private TextMeshProUGUI dialogSentenceText;
    private bool isFinishedTyping = false;
    private readonly float typingSpeed = 0.05f;
    bool isPlayerInTrigger = false;
    [SerializeField]
    private GameObject objectCanvas;

    [SerializeField]
    private Button actionButton;

    bool isDialoging = false;

    GameObject player;

    private void Update()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }

        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.R) && isFinishedTyping == false)
        {
            ActionButton();
        }

        if (isDialoging)
        {
            objectCanvas.SetActive(false);
        }
    }

    private void ActionButton()
    {
        string playerName = PlayerPrefsController.instance.GetCharacterName();
        StartCoroutine(DialogSequence(playerName));
    }

    private IEnumerator DialogSequence(string playerName)
    {
        isDialoging = true;
        player.GetComponent<PlayerController>().canMove = false;
        yield return StartCoroutine(OpenDialogPanel("Emak", $"{playerName}, tolongin emak belanja bentar."));
        yield return StartCoroutine(OpenDialogPanel("Emak", "Nih gocap, beliin emak cabe rawit 1 ons, bawang merah sama putih setengah ons, sayur kangkung 2 iket, saos tiram 1 saset, Royco 2 saset, terus..."));
        yield return StartCoroutine(OpenDialogPanel("Emak", "Apalagi ya?..."));
        yield return StartCoroutine(OpenDialogPanel(playerName, "Buset mak, ini mah too many..."));
        yield return StartCoroutine(OpenDialogPanel("Emak", "Apaan tuh tumani? sama ini anu, minyak goreng, udah cepetan beli!!  "));
        yield return StartCoroutine(OpenDialogPanel(playerName, "Oke, mak!"));
        yield return StartCoroutine(OpenDialogPanel("Emak", "Hati-hati di jalan, jangan jajan!"));
        dialogPanel.SetTrigger("Hide");
        isDialoging = false;
        player.GetComponent<PlayerController>().canMove = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true;
            objectCanvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            objectCanvas.SetActive(false);
        }
    }

    public IEnumerator OpenDialogPanel(string nameText, string sentenceText)
    {
        dialogPanel.gameObject.SetActive(true);
        dialogNameText.text = "";
        dialogSentenceText.text = "";
        dialogPanel.SetTrigger("Show");
        yield return new WaitForSeconds(1f);
        dialogNameText.text = nameText;
        dialogSentenceText.text = "";

        int wordIndex = 0;
        isFinishedTyping = false;

        while (isFinishedTyping == false)
        {
            dialogSentenceText.text += sentenceText[wordIndex];
            yield return new WaitForSeconds(typingSpeed);

            if (++wordIndex == sentenceText.Length)
            {
                isFinishedTyping = true;
                break;
            }
        }

        yield return new WaitForSeconds(1f);
    }
}
