using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private ushort _roomId;
        [SerializeField] private Player.Player _playerTemplate;
        private Dictionary<ushort, Player.Player> _players = new();

        public ushort RoomId => _roomId;
        
        private void Start()
        {
            RoomNetwork.CurrentRoom = this;
            RoomNetwork.Singleton.SendEnterGameRequest();
           // RoomNetwork.Singleton.GetAllPlayersRequest();
        }
        
        public void SpawnPlayer(ushort id, string nickname, Vector3 position)
        {
            var player = Instantiate(_playerTemplate, position, Quaternion.identity, transform);

            player.AddComponent<PlayerUpdater>();

            if (NetworkManager.Singleton.Client.Id == id)
                player.AddComponent<LocalPlayerController>();

            player.Init(id, nickname);

            Players.Dictionary[id] = player;
            _players[id] = player;
        }

        public void RemovePlayer(ushort id)
        {
            Destroy(_players[id].gameObject);
            Players.Dictionary.Remove(id);
            _players.Remove(id);
        } 
    }
}