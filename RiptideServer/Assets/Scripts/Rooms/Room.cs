using System.Collections.Generic;
using Chat;
using UnityEngine;

public enum TargetType
{
    player
}

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private ushort _roomId;
        [SerializeField] private Player.Player _defaultPlayer;
        [SerializeField] private Spell[] spellPatterns;
        
        private Dictionary<ushort, Player.Player> _players;
        private RoomChat _roomChat;
        
        private RoomNetwork _roomNetwork;

        public ushort RoomId => _roomId;
        public Dictionary<ushort, Player.Player>  Players => _players;
        private void Start()
        {
            _roomNetwork = new RoomNetwork(this);
            
            _players = new Dictionary<ushort, Player.Player>();
            _roomChat = new RoomChat();

            Rooms.Add(_roomId, this);
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
            Debug.Log($"New player added to {this.name}");
            _players[player.NetworkId] = player;
            player.ChangeRoom(this);
            
            _roomChat.AddPlayer(player.NetworkId);

            var playerTransform = player.transform;
            
            playerTransform.parent = transform;
            playerTransform.position = position;
            
            _roomNetwork.LoadRoomRequest(player.NetworkId);
            _roomNetwork.SendNewPlayer(player);
        }
        
        public void SpawnPlayer(ushort playerId, Vector2 position, string nickName)
        {
            Debug.Log($"New player spawned in {this.name}");
            var player = Instantiate(Player.Players.Dictionary.ContainsKey(playerId) ?
                Player.Players.Dictionary[playerId] : _defaultPlayer, position, Quaternion.identity, transform);

            player.Init(playerId, nickName, this);
            
            _roomNetwork.LoadRoomRequest(_roomId);

            _players[playerId] = player;
            Player.Players.Dictionary[playerId] = player;
            
            _roomChat.AddPlayer(player.NetworkId);
            
            _roomNetwork.SendNewPlayer(player);
        }

        public bool TryRemovePlayer(ushort networkID)
        {
            if (_players.Remove(networkID))
            {
                Debug.Log($"Removing player from {this.name}");
                _roomChat.RemovePlayer(networkID);
                
                _roomNetwork.SendRemovePlayer(networkID);
                
                return true;
            }
        
            return false;
        }

        public void SendAllPlayers(ushort playerId)
        {
            _roomNetwork.SendAllPlayers(playerId);
        }

        public void CreateSpell(Player.Player caster, ushort spellID, ushort targetID, ushort targetType)
        {
            FireBall fireBall = Instantiate((FireBall)spellPatterns[spellID], caster.transform.position, Quaternion.identity);
            switch (targetType)
            {
                case (ushort)TargetType.player:
                    if(_players.TryGetValue(targetID, out Player.Player target))
                    {
                        fireBall.Init(null, target.transform, this);
                    }
                    break;
                default: return;
            }
            
            //_roomNetwork.SendSpellCreated(0, spellID, new Vector2(caster.transform.localPosition.x, caster.transform.localPosition.y));
        }

        public void DestroySpell(ushort spellNetworkID)
        {
            _roomNetwork.SendSpellDestroyed(spellNetworkID);
        }
    }
}
    