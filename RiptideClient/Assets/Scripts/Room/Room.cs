using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
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
        
        public void SpawnPlayer(ushort id, string nickname, Vector3 position)
        {
            var player = Instantiate(_playerTemplate, position, Quaternion.identity);

            if (NetworkManager.Singleton.Client.Id == id)
                player.AddComponent<LocalPlayerUpdater>();
            else
                player.AddComponent<PlayerUpdater>();

            player.Init(id, nickname);

            player.transform.parent = transform;
            
            Players.Dictionary[id] = player;
            _players.Add(id, player);
        }
    }
}   