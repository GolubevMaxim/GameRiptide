using System.Collections.Generic;
using RiptideNetworking;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    [Header("Chat Log")]
    [SerializeField] private GameObject chatLog;
    [SerializeField] private GameObject chatMessageButtonPrefab;
    [SerializeField] private int maxMessageCount = 10;
    
    private Queue<GameObject> _chatMessageButtons;

    [SerializeField] public InputField chatField;

    private static Chat _singleton;

    public static Chat Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    private void Awake()
    {
        _singleton = this;
    }

    private void Start()
    {
        _chatMessageButtons = new Queue<GameObject>();
    }

    public void SendChatClicked()
    {
        if (chatField.text.Length > 0) SendChatMessage(chatField.text);
        chatField.text = "";
    }

    private void SendChatMessage(string chatMessage)
    {
        var message = Message.Create(MessageSendMode.reliable, ClientToServerId.Chat);
        
        message.AddString(chatMessage);
        
        NetworkManager.Singleton.Client.Send(message);
    }
    
    [MessageHandler((ushort) ServerToClientId.Chat)]
    private static void ReceiveChatMessages(Message message)
    {
        while (message.UnreadLength > 0)
        {
            ChatLogAdd(message.GetUShort(), message.GetString());
            
        }
    }
    
    public static void ChatLogAdd(int playerID, string str)
    {
        var button = Instantiate(Singleton.chatMessageButtonPrefab, Singleton.chatLog.transform, true);
        button.transform.GetComponent<Text>().text = $"{playerID}: {str}";
        Debug.Log($"{playerID}: {str}");
        LayoutRebuilder.ForceRebuildLayoutImmediate(Singleton.chatLog.GetComponent<RectTransform>());

        Singleton._chatMessageButtons.Enqueue(button);

        if (Singleton._chatMessageButtons.Count > Singleton.maxMessageCount)
        {
            var oldButton = Singleton._chatMessageButtons.Dequeue();
            Destroy(oldButton);
        }
    }
}