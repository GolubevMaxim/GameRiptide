using Player;
using Rooms;
using UnityEngine;


namespace Player
{
    
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour, Killable
    {
        private ushort _networkID;
        private string _nickName;
        private Room _currentRoom;
        private int _healthMax;
        private int _health;

        public ushort NetworkId => _networkID;
        public string NickName => _nickName;
        public Room CurrentRoom => _currentRoom;
        public int HealthMax => _healthMax;
        public int Health => _health;

        public void Init(ushort networkId, string nickName, Room room)
        {
            _networkID = networkId;
            _nickName = nickName;
            _currentRoom = room;
            _healthMax = 500;
            _health = _healthMax;
        }

        public Room GetRoom()
        {
            return _currentRoom;
        }

        public void ChangeRoom(Room newRoom)
        {
            _currentRoom = newRoom;
        }

        public void Leave()
        {
            Players.Dictionary.Remove(_networkID);
            _currentRoom.TryRemovePlayer(_networkID);
            
            Destroy(this);
        }

        public void ApplyDamage(int dmg)
        {
            _health -= dmg;
            if (_health < 0)
            {
                _health = 0;
                Die();
            }
        }

        public void ApplyHeal(int heal)
        {
            _health += heal;
            if (_health > _healthMax) _health = _healthMax;
        }

        public void Die()
        {
            Debug.Log($"Here {this.name} should have died.");
        }
    }
}   