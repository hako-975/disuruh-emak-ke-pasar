using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class BrowserPanel : MonoBehaviour
{
    [SerializeField]
    private SoundController soundController;

    [Header("Inputs")]
    [SerializeField]
    private TMP_InputField urlInput;

    [SerializeField]
    private TMP_InputField searchInput;

    [Header("Panels")]
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject jepangCitaPanel;

    [SerializeField]
    private GameObject jepangCitaRegisterPanel;

    [SerializeField]
    private GameObject jepangCitaLoginPanel;

    [SerializeField]
    private GameObject jepangCitaResetPasswordPanel;

    [SerializeField]
    private GameObject jepangCitaChangePasswordPanel;

    // Dashboard
    [SerializeField]
    private GameObject jepangCitaDashboardPanel;
    
    [SerializeField]
    private GameObject jepangCitaJadwalPanel;

    [SerializeField]
    private GameObject jepangCitaMateriPanel;


    [SerializeField]
    private GameObject notFoundPanel;


    [Header("Buttons")]
    [SerializeField]
    private Button goBackwardButton;

    [SerializeField]
    private Button goForwardButton;

    private GameObject currentPanel;
    private GameObject nextPanel;

    private readonly Stack<GameObject> panelHistory = new Stack<GameObject>();
    private readonly Stack<GameObject> panelFuture = new Stack<GameObject>();

    private MateriPanel materiPanel;

    void Start()
    {
        materiPanel = jepangCitaMateriPanel.GetComponent<MateriPanel>();

        goBackwardButton.interactable = false;
        goForwardButton.interactable = false;

        currentPanel = mainPanel;
        panelHistory.Push(currentPanel);

        goBackwardButton.onClick.AddListener(Backward);
        goForwardButton.onClick.AddListener(Forward);

        urlInput.onEndEdit.AddListener(OnInputEndEdit);
        searchInput.onEndEdit.AddListener(OnInputEndEdit);
    }

    private void OnInputEndEdit(string input)
    {
        soundController.PositiveButtonSound(gameObject);

        GameObject panelToShow;

        if (materiPanel.previewHurufPanelInstantiate != null)
        {
            Destroy(materiPanel.previewHurufPanelInstantiate);
            materiPanel.closeButton.onClick.RemoveAllListeners();
            materiPanel.closeButton.onClick.AddListener(delegate { materiPanel.BackToMateriPanel(true); });
        }

        if (IsJepangCitaInput(input))
        {
            panelToShow = jepangCitaPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                panelToShow = jepangCitaDashboardPanel;
            }
        }
        else if (IsJepangCitaDashboardInput(input))
        {
            panelToShow = jepangCitaPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                panelToShow = jepangCitaDashboardPanel;
            } 
        }
        else if (IsJepangCitaJadwalInput(input))
        {
            panelToShow = jepangCitaPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                panelToShow = jepangCitaJadwalPanel;
                // misi kedua
                PlayerPrefsController.instance.SetMission(1, soundController);
            }
        }
        else if (IsJepangCitaMateriInput(input))
        {
            panelToShow = jepangCitaPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                // misi kelima
                PlayerPrefsController.instance.SetMission(4, soundController);
                panelToShow = jepangCitaMateriPanel;
                materiPanel.BackToMateriPanel();
            }
        }
        else if (IsJepangCitaRegisterInput(input))
        {
            panelToShow = jepangCitaRegisterPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                panelToShow = jepangCitaDashboardPanel;
            }
        }
        else if (IsJepangCitaLoginInput(input))
        {
            panelToShow = jepangCitaLoginPanel;
            
            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                panelToShow = jepangCitaDashboardPanel;
            }
        }
        else if (IsJepangCitaResetPasswordInput(input))
        {
            panelToShow = jepangCitaResetPasswordPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 1)
            {
                panelToShow = jepangCitaDashboardPanel;
            }
        }
        else if (IsJepangCitaChangePasswordInput(input))
        {
            panelToShow = jepangCitaPanel;

            if (PlayerPrefsController.instance.GetCredentialJepangCita() == 2)
            {
                panelToShow = jepangCitaChangePasswordPanel;
            }
        }
        else if (IsTemukanInput(input))
        {
            panelToShow = mainPanel;
        }
        else
        {
            panelToShow = notFoundPanel;
        }

        if (panelToShow != currentPanel)
        {
            nextPanel = panelToShow;
            UpdatePanel();
        }
    }

    private bool IsJepangCitaInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        if (lowercaseInput.Contains("httpspasarcomdashboard") || lowercaseInput.Contains("pasarcomdashboard"))
        {
            return false;
        }
        else if (lowercaseInput.Contains("httpspasarcomjadwal") || lowercaseInput.Contains("pasarcomjadwal"))
        {
            return false;
        }
        else if (lowercaseInput.Contains("httpspasarcommateri") || lowercaseInput.Contains("pasarcommateri"))
        {
            return false;
        }
        else if (lowercaseInput.Contains("httpspasarcomlogin") || lowercaseInput.Contains("pasarcomlogin"))
        {
            return false;
        }
        else if (lowercaseInput.Contains("httpspasarcomresetpassword") || lowercaseInput.Contains("pasarcomresetpassword"))
        {
            return false;
        }
        else if (lowercaseInput.Contains("httpspasarcomchangepassword") || lowercaseInput.Contains("pasarcomchangepassword"))
        {
            return false;
        }
        else if (lowercaseInput.Contains("httpspasarcomregister") || lowercaseInput.Contains("pasarcomregister"))
        {
            return false;
        }
        return lowercaseInput.Contains("pasar") || lowercaseInput.Contains("pasar") || lowercaseInput.Contains("pasarcom") || lowercaseInput.Contains("httpspasarcom");
    }

    private bool IsJepangCitaDashboardInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasardashboard") || lowercaseInput.Contains("pasar dashboard") || lowercaseInput.Contains("pasarcomdashboard") || lowercaseInput.Contains("httpspasarcomdashboard");
    }

    private bool IsJepangCitaJadwalInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasarjadwal") || lowercaseInput.Contains("pasar jadwal") || lowercaseInput.Contains("pasarcomjadwal") || lowercaseInput.Contains("httpspasarcomjadwal");
    }

    private bool IsJepangCitaMateriInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasarmateri") || lowercaseInput.Contains("pasar materi") || lowercaseInput.Contains("pasarcommateri") || lowercaseInput.Contains("httpspasarcommateri");
    }

    private bool IsJepangCitaRegisterInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasarregister") || lowercaseInput.Contains("pasar register") || lowercaseInput.Contains("pasarcomregister") || lowercaseInput.Contains("httpspasarcomregister");
    }

    private bool IsJepangCitaLoginInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasarlogin") || lowercaseInput.Contains("pasar login") || lowercaseInput.Contains("pasarcomlogin") || lowercaseInput.Contains("httpspasarcomlogin");
    }

    private bool IsJepangCitaResetPasswordInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasarresetpassword") || lowercaseInput.Contains("pasar resetpassword") || lowercaseInput.Contains("pasarcomresetpassword") || lowercaseInput.Contains("httpspasarcomresetpassword");
    }

    private bool IsJepangCitaChangePasswordInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("pasarchangepassword") || lowercaseInput.Contains("pasar changepassword") || lowercaseInput.Contains("pasarcomchangepassword") || lowercaseInput.Contains("httpspasarcomchangepassword");
    }

    private bool IsTemukanInput(string input)
    {
        string lowercaseInput = Regex.Replace(input, "[^a-zA-Z0-9]", "").Trim().ToLower();
        return lowercaseInput.Contains("temukan") || lowercaseInput.Contains("temukancom") || lowercaseInput.Contains("httpstemukancom");
    }

    private IEnumerator AnimationOpenWeb(GameObject panel)
    {
        panel.SetActive(true);
        panel.GetComponent<Animator>().SetTrigger("Show");
        yield return null;
    }

    private IEnumerator AnimationCloseWeb(GameObject panel)
    {
        panel.GetComponent<Animator>().SetTrigger("Hide");
        yield return null;
    }

    private void UpdatePanel()
    {
        goBackwardButton.interactable = panelHistory.Count > 0;

        if (nextPanel != null)
        {
            panelHistory.Push(currentPanel);

            if (currentPanel != null)
            {
                StartCoroutine(AnimationCloseWeb(currentPanel));
            }

            currentPanel = nextPanel;
            StartCoroutine(AnimationOpenWeb(nextPanel));
            goForwardButton.interactable = false;
            panelFuture.Clear(); 
        }
        else
        {
            goForwardButton.interactable = panelFuture.Count > 0;
        }

        UpdateUrlInputText();
    }

    private void Backward()
    {
        soundController.PositiveButtonSound(gameObject);

        if (panelHistory.Count > 1)
        {
            panelFuture.Push(currentPanel);

            nextPanel = panelHistory.Pop();
            StartCoroutine(AnimationCloseWeb(currentPanel));
            StartCoroutine(AnimationOpenWeb(nextPanel));

            currentPanel = nextPanel;
            goForwardButton.interactable = true; 
        }

        goBackwardButton.interactable = panelHistory.Count > 1 || currentPanel != mainPanel;
        UpdateUrlInputText();
    }


    private void Forward()
    {
        soundController.PositiveButtonSound(gameObject);

        if (panelFuture.Count > 0)
        {
            panelHistory.Push(currentPanel);

            nextPanel = panelFuture.Pop();
            StartCoroutine(AnimationCloseWeb(currentPanel));
            StartCoroutine(AnimationOpenWeb(nextPanel));

            currentPanel = nextPanel;
            goBackwardButton.interactable = true;
        }

        goForwardButton.interactable = panelFuture.Count > 0;
        UpdateUrlInputText();
    }

    private void UpdateUrlInputText()
    {
        if (currentPanel == jepangCitaPanel)
        {
            urlInput.text = "https://pasar.com/";
        }
        else if (currentPanel == jepangCitaDashboardPanel)
        {
            urlInput.text = "https://pasar.com/dashboard";
        }
        else if (currentPanel == jepangCitaJadwalPanel)
        {
            urlInput.text = "https://pasar.com/jadwal";
        }
        else if (currentPanel == jepangCitaMateriPanel)
        {
            urlInput.text = "https://pasar.com/materi";
        }
        else if (currentPanel == jepangCitaLoginPanel)
        {
            urlInput.text = "https://pasar.com/login";
        }
        else if (currentPanel == jepangCitaResetPasswordPanel)
        {
            urlInput.text = "https://pasar.com/resetpassword";
        }
        else if (currentPanel == jepangCitaChangePasswordPanel)
        {
            urlInput.text = "https://pasar.com/changepassword";
        }
        else if (currentPanel == jepangCitaRegisterPanel)
        {
            urlInput.text = "https://pasar.com/register";
        }
        else if (currentPanel == mainPanel)
        {
            urlInput.text = "https://temukan.com/";
        }
    }

    public void OnLogoNavButtonClick()
    {
        OnInputEndEdit("https://pasar.com");
    }

    public void OnDashboardNavButtonClick()
    {
        OnInputEndEdit("https://pasar.com/dashboard");
    }

    public void OnRegisterNavButtonClick()
    {
        OnInputEndEdit("https://pasar.com/register");
    }

    public void OnLoginNavButtonClick()
    {
        OnInputEndEdit("https://pasar.com/login");
    }

    public void OnJadwalNavButtonClick()
    {
        OnInputEndEdit("https://pasar.com/jadwal");
    }

    public void OnMateriNavButtonClick()
    {
        OnInputEndEdit("https://pasar.com/materi");
    }

    public void OnResetPasswordButtonClick()
    {
        OnInputEndEdit("https://pasar.com/resetpassword");
    }

    public void OnChangePasswordButtonClick()
    {
        OnInputEndEdit("https://pasar.com/changepassword");
    }

    public void OnLogoutNavButtonClick()
    {
        PlayerPrefsController.instance.SetCredentialJepangCita(0);
        OnInputEndEdit("https://pasar.com/login");
    }
}
