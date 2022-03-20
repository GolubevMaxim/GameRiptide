using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using UnityEngine;

public enum ClientToServerId : ushort
{
    logpas = 1,
    enterGame,
    leaveGame,
    chat
}

public enum ServerToClientId : ushort
{
    logpas = 1,
    roomData,
    chat
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    public Client client { get; private set; }
    [SerializeField] private string ip;
    [SerializeField] private string port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        client = new Client();
        client.Connected += DidConnect;
        client.ConnectionFailed += FailToConnect;
        client.Disconnected += DidDisconnect;
        DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        client.Tick();
    }

    private void OnApplicationQuit()
    {
        client.Disconnect();
    }

    public void Connect()
    {
        client.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        SendLogPas();
    }

    private void FailToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SetUI((int)UIs.authorization);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SetUI((int)UIs.authorization);
    }

    [MessageHandler((ushort)ServerToClientId.logpas)]
    private static void RecieveLogPas(Message message)
    {
        if (message.GetBool())
        {
            Debug.Log("Sucessfuly authorized! Your id: " + message.GetInt());
            UIManager.Singleton.SetUI((int)UIs.character);
        }
        else
        {
            Debug.Log("Wrong login or password.");
        }
    }

    public void SendLogPas()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.logpas);
        message.AddString(UIManager.Singleton.loginField.text);
        message.AddString(UIManager.Singleton.passwordField.text);
        client.Send(message);
    }

    public void SendEnterGameRequest()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.enterGame);
        message.AddInt(0);
        client.Send(message);
    }

    public void SendLeaveGameRequest()
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.leaveGame);
        client.Send(message);
        UIManager.Singleton.SetUI((int)UIs.character);
        GameManager.SetRoom("Menu");
    }

    [MessageHandler((ushort)ServerToClientId.roomData)]
    private static void RecieveRoomData(Message message)
    {
        int roomNum = message.GetInt();
        Debug.Log($"Got room number {roomNum}.");
        UIManager.Singleton.SetUI((int)UIs.game);
        GameManager.SetRoom("Room testing");
    }

    public void SendChatMessage(string str)
    {
        Message message = Message.Create(MessageSendMode.reliable, ClientToServerId.chat);
        message.AddString(str);
        client.Send(message);
    }

    [MessageHandler((ushort)ServerToClientId.chat)]
    private static void RecieveChatMessages(Message message)
    {
        while(message.UnreadLength > 0)
        {
            UIManager.Singleton.ChatLogAdd(message.GetInt(), message.GetString());
        }
    }
}
