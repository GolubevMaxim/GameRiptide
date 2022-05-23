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
        [SerializeField] private Spell[] _spellTemplates;
        private readonly Dictionary<ushort, Player.Player> _players = new();
        private readonly Dictionary<ushort, Spell> _spells = new();
        public ushort RoomId => _roomId;
        
        private void Start()
        {
            RoomNetwork.CurrentRoom = this;
            RoomNetwork.Singleton.SendEnterGameRequest();
        }
        
        public void SpawnPlayer(ushort id, string nickname, Vector3 position, int healthMax, int health)
        {
            var player = Instantiate(_playerTemplate, position, Quaternion.identity, transform);

            if (NetworkManager.Singleton.Client.Id == id)
            {
                if (Camera.main != null) 
                    Camera.main.GetComponent<CameraController>().target = player.transform;
                player.AddComponent<LocalPlayerController>();
            }

            player.Init(id, nickname, healthMax, health);

            Players.Dictionary[id] = player;
            _players[id] = player;
        }

        public void RemovePlayer(ushort id)
        {
            Destroy(_players[id].gameObject);
            
            Players.Dictionary.Remove(id);
            _players.Remove(id);
        }

        public void CreateSpell(ushort networkID, ushort id, Player.Player caster)
        {
            Spell spell = Instantiate(_spellTemplates[id], caster.transform.position, Quaternion.identity);
            spell.Init(networkID, id);
            _spells.Add(networkID, spell);
        }

        public void UpdateSpell(ushort networkID, float x, float y)
        {
            if(_spells.TryGetValue(networkID, out Spell spell))
            {
                spell.UpdatePosition(x, y);
            }
        }

        public void DestroySpell(ushort networkID)
        {
            if(_spells.TryGetValue(networkID, out Spell spell))
            {
                _spells.Remove(networkID);
                spell.Destroy();
            }
        }
    }
}