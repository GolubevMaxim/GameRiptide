using System.Collections.Generic;
using UnityEngine;

namespace Room
{
    public class Room : MonoBehaviour
    {
        [SerializeField] private Player.Player _playerTemplate;
        private Dictionary<ushort, Player.Player> _players = new();
        private void Start()
        {
            Debug.Log("Scene loaded!");
            
            RoomNetwork.CurrentRoom = this;
            RoomNetwork.Singleton.SendEnterGameRequest();
        }
        
        public void AddPlayer(ushort id, string nickname, Vector3 position)
        {
            var player = Instantiate(_playerTemplate, position, Quaternion.identity);
            player.Init(id, nickname);
            Player.Players.Dictionary[id] = player;
            _players.Add(id, player);
        }
    }
}