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

    public Server server { get; private set; }
    [SerializeField] private ushort port;
    [SerializeField] private ushort maxClientCount;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        server = new Server();
        server.ClientDisconnected += OnClientDisconnected;
        server.Start(port, maxClientCount);
    }

    private void FixedUpdate()
    {
        server.Tick();
        foreach (Room room in GameManager.Singleton.rooms) room.SendMessages();
    }

    private void OnApplicationQuit()
    {
        server.Stop();
    }

    
    private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        GameManager.Singleton.RemoveUser(e.Id);
    }

    [MessageHandler((ushort)ClientToServerId.logpas)]
    private static void RecieveLogPas(ushort fromClientId, Message message)
    {
        User user = 
        DataBaseManager.SelectPlayerLogPas(message.GetString(), message.GetString());

        SendAnswerLogPas(user != null, fromClientId, user);
        if(user == null) Singleton.server.DisconnectClient(fromClientId);
        else
        {
            GameManager.Singleton.users.Add(fromClientId, user);
            GameManager.Singleton.usersInMenu.Add(fromClientId, user);
        }
    }

    private static void SendAnswerLogPas(bool userExists, ushort userId, User user)
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.logpas);
        message.AddBool(userExists);
        if (userExists)
        {
            message.AddInt(user.id);
        }
        Singleton.server.Send(message, userId);
    }

    [MessageHandler((ushort)ClientToServerId.enterGame)]
    private static void RecieveEnterGameRequest(ushort fromClientId, Message message)
    {
        if (GameManager.Singleton.AddUserToGame(fromClientId))
        {
            SendRoomData(fromClientId, 0);
        }
    }

    private static void SendRoomData(ushort userNetworkId, int roomNum)
    {
        Message message = Message.Create(MessageSendMode.reliable, ServerToClientId.roomData);
        message.AddInt(roomNum);
        Singleton.server.Send(message, userNetworkId);
    }

    [MessageHandler((ushort)ClientToServerId.leaveGame)]
    private static void RecieveLeaveGameRequest(ushort fromClientId, Message message)
    {
        GameManager.Singleton.RemoveUserFromGame(fromClientId);
    }

    [MessageHandler((ushort)ClientToServerId.chat)]
    private static void RecieveChatMessage(ushort fromClientId, Message message)
    {
        User user;
        if (GameManager.Singleton.users.TryGetValue(fromClientId, out user))
        {
            user.getRoom().AddMessageToBuffer(user.id, message.GetString());
            user.getRoom().bufferEmpty = false;
        }
    }
}
