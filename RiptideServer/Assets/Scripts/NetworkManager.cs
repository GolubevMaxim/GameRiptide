using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public enum ClientToServerId : ushort
{
    Logpas = 1,
    EnterGame,
    LeaveGame,
    Chat
}

public enum ServerToClientId : ushort
{
    Logpas = 1,
    RoomData,
    Chat
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

    public Server Server { get; private set; }
    
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

        Server = new Server();
    
        Server.ClientDisconnected += OnClientDisconnected;
        
        Server.Start(port, maxClientCount);
    }

    private void FixedUpdate()
    {
        Server.Tick();

        foreach (var room in GameManager.Singleton.Rooms) room.SendChat();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    
    private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        GameManager.Singleton.RemoveUser(e.Id);
    }

    [MessageHandler((ushort) ClientToServerId.Logpas)]
    private static void ReceiveLogPas(ushort fromClientId, Message message)
    {
        var user = DataBaseManager.SelectPlayerLogPas(message.GetString(), message.GetString());

        SendAnswerLogPas(user != null, fromClientId, user);
        
        if (user == null)
        {
            Singleton.Server.DisconnectClient(fromClientId);
        }
        else
        {
            GameManager.Singleton.Users.Add(fromClientId, user);
            GameManager.Singleton.UsersInMenu.Add(fromClientId, user);
        }
    }

    private static void SendAnswerLogPas(bool userExists, ushort userId, User user)
    {
        var message = Message.Create(MessageSendMode.reliable, ServerToClientId.Logpas);
        
        message.AddBool(userExists);
        
        if (userExists)
        {
            message.AddInt(user.id);
        }
        
        Singleton.Server.Send(message, userId);
    }

    [MessageHandler((ushort) ClientToServerId.EnterGame)]
    private static void ReceiveEnterGameRequest(ushort fromClientId, Message message)
    {
        if (GameManager.Singleton.AddUserToGame(fromClientId))
        {
            SendRoomData(fromClientId, 0);
        }
    }

    private static void SendRoomData(ushort userNetworkId, int roomNum)
    {
        var message = Message.Create(MessageSendMode.reliable, ServerToClientId.RoomData);
        
        message.AddInt(roomNum);
        
        Singleton.Server.Send(message, userNetworkId);
    }

    [MessageHandler((ushort) ClientToServerId.LeaveGame)]
    private static void ReceiveLeaveGameRequest(ushort fromClientId, Message message)
    {
        GameManager.Singleton.RemoveUserFromGame(fromClientId);
    }

    [MessageHandler((ushort) ClientToServerId.Chat)]
    private static void ReceiveChatMessage(ushort fromClientId, Message message)
    {
        if (!GameManager.Singleton.Users.TryGetValue(fromClientId, out var user)) return;
        
        user.GetRoom().AddMessageToChat(user.id, message.GetString());
    }
}
