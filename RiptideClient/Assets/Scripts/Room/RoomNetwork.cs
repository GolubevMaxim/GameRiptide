using RiptideNetworking;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Room
{
    public class RoomNetwork : MonoBehaviour
    {
        public static Room CurrentRoom;
        
        private static RoomNetwork _singleton;
        
        public static RoomNetwork Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(RoomNetwork)} instance already exists, destroying duplicate.");
                    Destroy(value);
                }
            }
        }

        private void Awake()
        {
            Singleton = this;
        }

        public void SendEnterGameRequest()
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.EnterGame);
        
            message.AddUShort(CurrentRoom.RoomId);
            message.AddVector2(new Vector2(0, 0));
            message.AddString("playerName");
            
            NetworkManager.Singleton.Client.Send(message);
        }

        public void SendLeaveGameRequest()
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.LeaveGame);
        
            NetworkManager.Singleton.Client.Send(message);
        
            UIManager.Singleton.SetUI((int)UIs.Character);
            SceneManager.LoadScene("Menu");
        }

        [MessageHandler((ushort) ServerToClientId.RoomData)]
        private static void ReceiveRoomData(Message message)
        {
            var roomNum = message.GetInt();
        
            Debug.Log($"Got room number {roomNum}.");
        
            UIManager.Singleton.SetUI((int)UIs.Game);
        }

        [MessageHandler((ushort)ServerToClientId.RoomPlayers)]
        private static void ReceiveRoomPlayers(Message message)
        {
            if (CurrentRoom == null) return;
            
            while (message.UnreadLength > 0)
            {
                var playerId = message.GetUShort();
                var playerName = message.GetString();
                var playerPosition = new Vector3(message.GetFloat(), message.GetFloat(), 0);
                var playerHealthMax = message.GetInt();
                var playerHealth = message.GetInt();
                
                CurrentRoom.SpawnPlayer(playerId, playerName, playerPosition, playerHealthMax, playerHealth);
            }
        }
        
        [MessageHandler((ushort)ServerToClientId.RemovePlayerFromRoom)]
        private static void RemovePlayerFromRoom(Message message)
        {
            var playerId = message.GetUShort();
            
            CurrentRoom.RemovePlayer(playerId);
        }

        [MessageHandler((ushort) ServerToClientId.LoadRoom)]
        private static void LoadRoom(Message message)
        {
            var roomId = message.GetUShort();

            if (SceneManager.GetActiveScene().name != $"room{roomId}")
                SceneManager.LoadScene($"room{roomId}");
        }

        public void GetAllPlayersRequest()
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.AllPlayersPosition);
            
            NetworkManager.Singleton.Client.Send(message);
        }
    }
}