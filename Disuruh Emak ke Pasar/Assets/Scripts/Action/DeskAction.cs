using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DeskAction : MonoBehaviour
{
    [SerializeField]
    private SoundController soundController;

    private Camera mainCam;

    [SerializeField]
    private Camera actionCam;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject deskCanvas;

    [SerializeField]
    private GameObject deskPanel;

    [SerializeField]
    private Button actionButton;

    [SerializeField]
    private Button shutdownButton;

    [SerializeField]
    private Animator laptopAnimator;

    private ActionController actionController;

    private GameObject player;

    private PlayerController playerController;

    bool isPlayerInTrigger = false;

    bool isOpen = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        actionController = GetComponentInParent<ActionController>();
        playerController = player.GetComponent<PlayerController>();
        mainCam = FindObjectOfType<Camera>();
        deskCanvas.SetActive(false);
        deskPanel.SetActive(false);

        shutdownButton.onClick.AddListener(ShutdownButton);
        actionButton.onClick.AddListener(ActionButton);
    }

    void Update()
    {
        if (isPlayerInTrigger && Input.GetKeyDown(KeyCode.R) && isOpen == false)
        {
            ActionButton();
            Cursor.lockState = CursorLockMode.Confined;
        }

        if (actionController.deskIsActive)
        {
            deskCanvas.SetActive(true);
            Time.timeScale = 1f;
        }
    }

    private void ShutdownButton()
    {
        isOpen = false;

        soundController.ShutdownSound(gameObject);
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(WaitLaptopClose());
    }

    private IEnumerator WaitLaptopClose()
    {
        actionController.deskIsActive = false;
        playerController.canMove = true;

        canvas.GetComponent<CanvasGroup>().alpha = 1;
        canvas.GetComponent<CanvasGroup>().interactable = true;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = true;

        actionButton.gameObject.SetActive(true);
        deskPanel.SetActive(false);
        laptopAnimator.SetTrigger("LaptopClose");
        yield return new WaitForSeconds(0.5f);
        soundController.CloseSound(gameObject);
        yield return new WaitForSeconds(1f);
        actionCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
    }

    private void ActionButton()
    {
        isOpen = true;
        soundController.OpenSound(gameObject);
        StartCoroutine(WaitDeskPanelOpen());
    }

    private IEnumerator WaitDeskPanelOpen()
    {
        actionController.deskIsActive = true;
        playerController.canMove = false;

        canvas.GetComponent<CanvasGroup>().alpha = 0;
        canvas.GetComponent<CanvasGroup>().interactable = false;
        canvas.GetComponent<CanvasGroup>().blocksRaycasts = false;

        mainCam.gameObject.SetActive(false);

        actionButton.gameObject.SetActive(false);

        actionCam.gameObject.SetActive(true);

        laptopAnimator.SetTrigger("LaptopOpen");
        yield return new WaitForSeconds(1f);
        deskCanvas.SetActive(true);
        deskPanel.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = true;

            deskCanvas.SetActive(true);
            actionController.canvasTrigger = deskCanvas;
            actionController.isTriggerEntered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerInTrigger = false;

            deskPanel.SetActive(false);
            deskCanvas.SetActive(false);
            actionController.isTriggerEntered = false;
        }
    }
}
