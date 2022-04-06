using System.Collections.Generic;
using Chat;
using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private ushort roomId;
        [SerializeField] private Player.Player defaultPlayer;
        
        private Dictionary<ushort, Player.Player> _players;
        private RoomChat _roomChat;

        public Dictionary<ushort, Player.Player>  Players => _players;
        private RoomNetwork _roomNetwork;

        private void Start()
        {
            _roomNetwork = new RoomNetwork(this);
            
            _players = new Dictionary<ushort, Player.Player>();
            _roomChat = new RoomChat();

            Rooms.Add(roomId, this);
        }

        private void Update()
        {
            _roomNetwork.SendPlayerPositions();
        }

        public void AddMessageToChat(ushort playerID, string chatMessage)
        {
            _roomChat.AddMessageToBuffer(playerID, chatMessage);
        }

        public void SendChat()
        {
            _roomChat.SendMessages();
        }

        public void AddPlayer(Player.Player player, Vector2 position)
        {
            _players[player.NetworkId] = player;
            
            var playerTransform = player.transform;
            
            playerTransform.parent = transform;
            playerTransform.position = position;
        }
        
        public void SpawnPlayer(ushort playerId, Vector2 position, string nickName)
        {

            var player = Instantiate(Player.Players.Dictionary.ContainsKey(playerId) ?
                Player.Players.Dictionary[playerId] : defaultPlayer, position, Quaternion.identity, transform);

            player.Init(playerId, nickName, this);

            _players[playerId] = player;
            Player.Players.Dictionary[playerId] = player;
            
            _roomChat.AddUser(player.NetworkId);
            
            _roomNetwork.SendNewPlayer(player);
        }

        public bool TryRemovePlayer(ushort networkID)
        {
            if (_players.Remove(networkID))
            {
                _roomChat.RemoveUser(networkID);
                
                _roomNetwork.SendRemovePlayer(networkID);
                
                return true;
            }
        
            return false;
        }
    }
}
