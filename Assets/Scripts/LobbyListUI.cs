using System;
using System.Collections.Generic;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LobbyListUI : MonoBehaviour
{
    public static LobbyListUI Instance {  get; private set; }

    [SerializeField] private Button createLobbyButton;
    [SerializeField] private Button refreshLobbyButton;
    [SerializeField] private Transform lobbyListContainer;
    [SerializeField] private Transform lobbySingleTemplate;

    private void Awake()
    {
        Instance = this;
        Hide();

        lobbySingleTemplate.gameObject.SetActive(false);
    }
    private void Start()
    {
        createLobbyButton.onClick.AddListener(CreateLobbyButton);
        refreshLobbyButton.onClick.AddListener(RefreshButtonClick);
        LobbyManager.Instance.OnLobbyListChanged += LobbyManager_OnLobbyListChanged;
        LobbyManager.Instance.OnJoinedLobby += LobbyManager_OnJoinedLobby;
        LobbyManager.Instance.OnLeftLobby += LobbyManager_OnLeftLobby;
        LobbyManager.Instance.OnKickedFromLobby += LobbyManager_OnKickedFromLobby;
    }

    private void LobbyManager_OnKickedFromLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        Show();
    }

    private void LobbyManager_OnLeftLobby(object sender, EventArgs e)
    {
        Show();
    }

    private void LobbyManager_OnJoinedLobby(object sender, LobbyManager.LobbyEventArgs e)
    {
        Hide();
    }

    private void LobbyManager_OnLobbyListChanged(object sender, LobbyManager.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }
    private void CreateLobbyButton()
    {
        Hide();
        CreateLobbyUI.Instance.Show();
    }
    private void RefreshButtonClick()
    {
        LobbyManager.Instance.RefreshLobbyList();
    }
    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        Debug.Log("Lobby List Count : " + lobbyList.Count);
        foreach (Transform child in lobbyListContainer)
        {
            if (child == lobbySingleTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbySingleTransform = Instantiate(lobbySingleTemplate, lobbyListContainer);
            lobbySingleTransform.gameObject.SetActive(true);
            LobbyListSingleUI lobbyListSingleUI = lobbySingleTransform.GetComponent<LobbyListSingleUI>();
            lobbyListSingleUI.UpdateLobby(lobby);
        }
    }
    public void Hide()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void Show()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }
}
