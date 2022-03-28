using RiptideNetworking;
using RiptideNetworking.Utils;
using System;
using UnityEngine;

public enum ClientToServerId : ushort
{
    Logpas = 1,
    EnterGame,
    LoadFinished,
    LeaveGame,
    Chat
}

public enum ServerToClientId : ushort
{
    Logpas = 1,
    RoomData,
    Chat,
    RoomPlayers,
    RemovePlayerFromRoom
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

    public Client Client { get; private set; }
    
    [SerializeField] private string ip;
    [SerializeField] private string port;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        
        Client = new Client();
        
        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailToConnect;
        Client.Disconnected += DidDisconnect;
        
        DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect()
    {
        Client.Connect($"{ip}:{port}");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        SendLogPas();
    }

    private void FailToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SetUI((int) UIs.Authorization);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SetUI((int) UIs.Authorization);
    }

    [MessageHandler((ushort) ServerToClientId.Logpas)]
    private static void ReceiveLogPas(Message message)
    {
        if (message.GetBool())
        {
            Debug.Log($"Successfully authorized! Your id: {message.GetInt()}");
            UIManager.Singleton.SetUI((int) UIs.Character);
        }
        else
        {
            Debug.Log("Wrong login or password.");
        }
    }

    public void SendLogPas()
    {
        var message = Message.Create(MessageSendMode.reliable, ClientToServerId.Logpas);
        
        message.AddString(UIManager.Singleton.loginField.text);
        message.AddString(UIManager.Singleton.passwordField.text);
        
        Client.Send(message);
    }
}
