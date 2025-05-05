using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button exitErrorButton;
    [SerializeField] private GameObject exitPanel;

    public static string playerName = "";

    private void Awake()
    {
        exitErrorButton.onClick.AddListener(ExitButton);
        loginButton.onClick.AddListener(async () =>
        {
            loginButton.interactable = false;

            playerName = usernameField.text;

            InitializeServices();

        });

        usernameField.onValueChanged.AddListener(Usernamefield_OnValueChanged);
    }
    private async void InitializeServices()
    {
        InitializationOptions initializationOptions = new InitializationOptions();
        initializationOptions.SetProfile(playerName);

        await UnityServices.InitializeAsync(initializationOptions);

        AuthenticationService.Instance.SignedIn += () =>
        {
            // do nothing
            Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
            LobbyManager.Instance.UpdatePlayerName(playerName);
            LobbyManager.Instance.RefreshLobbyList();
            Hide();
            LobbyListUI.Instance.Show();
        };

        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void Usernamefield_OnValueChanged(string value)
    {
        if (value.Length > 2)
            loginButton.interactable = true;
        else
            loginButton.interactable = false;
    }


    private void Hide()
    {
        usernameField.gameObject.SetActive(false);
        loginButton.gameObject.SetActive(false);
    }

    private void ExitButton()
    {
        exitPanel.SetActive(false);
        usernameField.text = string.Empty;
        loginButton.interactable = true;
    }
}
