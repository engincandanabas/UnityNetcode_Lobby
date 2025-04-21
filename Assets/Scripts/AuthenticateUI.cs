using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticateUI : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private Button loginButton;

    public static string playerName="";
    private void Awake()
    {
        loginButton.onClick.AddListener(async () =>
        {
            playerName = usernameField.text;
            InitializationOptions initializationOptions = new InitializationOptions();
            initializationOptions.SetProfile(playerName);

            await UnityServices.InitializeAsync(initializationOptions);

            AuthenticationService.Instance.SignedIn += () => {
                // do nothing
                Debug.Log("Signed in! " + AuthenticationService.Instance.PlayerId);
                LobbyManager.Instance.UpdatePlayerName(playerName);
                Hide();
                
            };

            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        });

        usernameField.onValueChanged.AddListener(Usernamefield_OnValueChanged);
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
}
