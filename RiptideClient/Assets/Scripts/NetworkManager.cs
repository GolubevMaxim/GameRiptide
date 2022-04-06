using RiptideNetworking.Utils;
using System;
using Login;
using UnityEngine;

public enum ClientToServerId : ushort
{
    Logpas = 1,
    EnterGame,
    LoadFinished,
    LeaveGame,
    Chat,
    DirectionInput,
}

public enum ServerToClientId : ushort
{
    Logpas = 1,
    RoomData,
    Chat,
    RoomPlayers,
    RemovePlayerFromRoom,
    PlayerPositionChange,
}

public class NetworkManager : MonoBehaviour
{
    public RiptideNetworking.Client Client { get; private set; }
    
    [SerializeField] private string ip;
    [SerializeField] private string port;
    
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

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        
        Client = new RiptideNetworking.Client();
        
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
        LoginNetwork.SendLogPas();
    }

    private void FailToConnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SetUI((int) UIs.Authorization);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        UIManager.Singleton.SetUI((int) UIs.Authorization);
    }
}
