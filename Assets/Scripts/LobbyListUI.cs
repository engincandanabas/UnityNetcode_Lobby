using UnityEngine;

public class LobbyListUI : MonoBehaviour
{
    public static LobbyListUI Instance {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }
    public void Hide()
    {

    }
    public void Show()
    {

    }
}
