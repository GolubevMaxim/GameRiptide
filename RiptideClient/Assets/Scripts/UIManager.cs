using RiptideNetworking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIs
{
    authorization = 0,
    character,
    game
}

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    [SerializeField] private GameObject canvas;
    [Header("UIs")]
    [SerializeField] private GameObject activeUI;
    [SerializeField] private GameObject[] menuUIs;
    [Header("Input fields")]
    [SerializeField] public InputField loginField;
    [SerializeField] public InputField passwordField;
    [SerializeField] public InputField chatField;
    [Header("Chat Log")]
    [SerializeField] private GameObject chatLog;
    [SerializeField] private GameObject chatMessageButtonPrefab;
    [SerializeField] private int maxMessageCount = 10;
    private Queue<GameObject> chatMessageButtons;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        DontDestroyOnLoad(canvas);
        foreach (GameObject ui in menuUIs){
            ui.SetActive(false);
        }
        activeUI.SetActive(true);

        loginField.text = "user0";
        passwordField.text = "123456789";

        chatMessageButtons = new Queue<GameObject>();
    }

    public void ConnectClicked()
    {
        SetUI((int) UIs.character);

        NetworkManager.Singleton.Connect();
    }

    public void SendChatClicked()
    {
        if (chatField.text.Length > 0) NetworkManager.Singleton.SendChatMessage(chatField.text);
        chatField.text = "";
    }

    public void SetUI(int num)
    {
        activeUI.SetActive(false);
        if (num < 0 || num > menuUIs.Length) return;
        activeUI = menuUIs[num];
        activeUI.SetActive(true);
    }

    public void ChatLogAdd(int userid, string str)
    {
        GameObject button = Instantiate(chatMessageButtonPrefab);
        button.transform.GetComponent<Text>().text = $"{userid}: {str}";
        button.transform.SetParent(chatLog.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(chatLog.GetComponent<RectTransform>());

        chatMessageButtons.Enqueue(button);

        if (chatMessageButtons.Count > maxMessageCount)
        {
            GameObject oldbutton = chatMessageButtons.Dequeue();
            Destroy(oldbutton);
        }
    }
}
