using System.Collections.Generic;
using Chat;
using Enemies;
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
        [SerializeField] private Enemy _defaultEnemy;
        
        [SerializeField] private Spell[] spellPatterns;
        
        private Spells _spells;
        private ushort _idcounter;

        private ushort _lastEnemyId = 0;

        private Dictionary<ushort, Player.Player> _players;
        private Dictionary<ushort, Enemy> _enemies;
        
        private RoomChat _roomChat;
        
        private RoomNetwork _roomNetwork;

        public ushort RoomId => _roomId;
        
        public Dictionary<ushort, Player.Player>  Players => _players;
        public Dictionary<ushort, Enemy> Enemies => _enemies;
        
        public Spells Spells => _spells;
        private void Start()
        {
            _roomNetwork = new RoomNetwork(this);
            
            _players = new Dictionary<ushort, Player.Player>();
            _enemies = new Dictionary<ushort, Enemy>();
            
            _roomChat = new RoomChat();

            Rooms.Add(_roomId, this);
            _spells = GetComponent<Spells>();
            _idcounter = 0;
            
            SummonEnemy();
        }

        private void Update()
        {
            _roomNetwork.SendPlayerPositions();
            _roomNetwork.SendSpellUpdatePos();
            _roomNetwork.SendEnemies();
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
            Transform target = null;
            switch (targetType)
            {
                case (ushort)TargetType.player:
                    if(_players.TryGetValue(targetID, out Player.Player player))
                    {
                        target = player.transform;
                    }
                    break;
                default: return;
            }
            switch (spellID)
            {
                case 0:
                    FireBall fireBall = Instantiate((FireBall)spellPatterns[spellID], caster.transform.position, Quaternion.identity, transform);
                    fireBall.Init(caster.transform, target, this, _idcounter);
                    _spells.AddUpdateble(fireBall);
                    break;
                default: return;
            }
            if (_spells.GetSpell(_idcounter) != null)
            {
                _roomNetwork.SendSpellCreated(_idcounter, spellID, caster);
                if (_idcounter == ushort.MaxValue) _idcounter = 0;
                else _idcounter++;
            }
        }

        public void DestroySpell(ushort spellNetworkID)
        {
            _spells.Remove(spellNetworkID);
            _roomNetwork.SendSpellDestroyed(spellNetworkID);
        }

        public void SummonEnemy()
        {
            var enemy = Instantiate(_defaultEnemy, Vector3.zero, Quaternion.identity, transform);
            enemy.transform.localPosition = new Vector3(10, 10, 0);
            enemy.Init(_lastEnemyId, this);
            
            _enemies[_lastEnemyId++] = enemy;
        }

        public void DestroyEnemy(ushort id)
        {
            var enemy = _enemies[id];
            _enemies.Remove(id);
            
            Destroy(enemy.gameObject);
            _roomNetwork.EnemyDie(id);
        }
    }
}
    