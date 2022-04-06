using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;

public enum ClientToServerId : ushort
{
    Logpas = 1,
    EnterGame,
    LoadFinished,
    LeaveGame,
    Chat,
    DirectionInput
}

public enum ServerToClientId : ushort
{
    Logpas = 1,
    RoomData,
    Chat,
    RoomPlayers,
    RemovePlayerFromRoom,
    PlayerPositionChange
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
        
        foreach (var room in Rooms.Rooms.Dictionary.Values)
            room.SendChat();
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    
    private void OnClientDisconnected(object sender, ClientDisconnectedEventArgs e)
    {
        if (Player.Players.Dictionary.TryGetValue(e.Id, out var player))
            player.Leave();
    }
    
    [MessageHandler((ushort) ClientToServerId.EnterGame)]
    private static void ReceiveEnterGameRequest(ushort fromClientId, Message message)
    {
        var roomIndex = message.GetUShort();
        var spawnPosition = message.GetVector2();
        var nickName = message.GetString();
        
        Rooms.Rooms.Dictionary[roomIndex].SpawnPlayer(fromClientId, spawnPosition, nickName);
        
        SendRoomData(fromClientId, 0);
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
        Singleton.Server.DisconnectClient(fromClientId);
    }
}
