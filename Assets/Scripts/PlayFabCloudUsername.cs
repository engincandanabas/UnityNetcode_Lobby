using PlayFab;
using PlayFab.CloudScriptModels;
using UnityEngine;
using System;
using System.Collections.Generic;
using PlayFab.ClientModels;

public class PlayFabCloudUsername : MonoBehaviour
{
    public static PlayFabCloudUsername Instance {  get; private set; }
    private void Awake()
    {
        Instance = this;

        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnFailure);
    }
    // Kullanýcý adýnýn mevcut olup olmadýðýný kontrol eden fonksiyon
    public void CheckUsername(string username, Action<bool> callback)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "CheckUsernameAvailability",
            FunctionParameter = new { username = username },
            GeneratePlayStreamEvent = true
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            if (result.FunctionResult != null)
            {
                Debug.Log(result.FunctionResult);
                try
                {
                    bool isTaken = Convert.ToBoolean(result.FunctionResult);
                    callback(!isTaken); // true: kullanýcý adý alýnmýþ, false: alýnmamýþ
                }
                catch (Exception e)
                {
                    Debug.LogError("Error parsing CloudScript result: " + e.Message);
                    callback(false); // Hata durumunda false dön
                }
            }
            else
            {
                Debug.LogError("CloudScript result is null.");
                callback(false);
            }
        }, error =>
        {
            Debug.LogError("Error executing CloudScript: " + error.GenerateErrorReport());
            callback(false);
        });
    }

    public void ReleaseUsername(string username)
    {
        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "ReleaseUsername",
            FunctionParameter = new { username = username },
            GeneratePlayStreamEvent = false
        };

        PlayFabClientAPI.ExecuteCloudScript(request, result =>
        {
            Debug.Log("Username released successfully.");
        }, error =>
        {
            Debug.LogError("Error releasing username: " + error.GenerateErrorReport());
        });
    }

    private void OnApplicationQuit()
    {
        if (!string.IsNullOrEmpty(AuthenticateUI.playerName))
        {
            ReleaseUsername(AuthenticateUI.playerName);
        }
    }


    private void OnSuccess(LoginResult result)
    {
        Debug.Log("PlayFab login successful! ID: " + result.PlayFabId);
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.LogError("PlayFab login failed: " + error.GenerateErrorReport());
    }
}
