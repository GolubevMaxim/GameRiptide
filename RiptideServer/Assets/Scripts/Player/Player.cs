using Player;
using Rooms;
using UnityEngine;


namespace Player
{
    
    [RequireComponent(typeof(PlayerMovement))]
    public class Player : MonoBehaviour
    {
        private ushort _networkID;
        private string _nickName;
        private Room _currentRoom;

        public ushort NetworkId => _networkID;
        public string NickName => _nickName;
        public Room CurrentRoom => _currentRoom;

        public void Init(ushort networkId, string nickName, Room room)
        {
            _networkID = networkId;
            _nickName = nickName;
            _currentRoom = room;
        }

        public void Leave()
        {
            Players.Dictionary.Remove(_networkID);
            _currentRoom.Players.Remove(_networkID);
            
            Destroy(this);
        }
    }
}   