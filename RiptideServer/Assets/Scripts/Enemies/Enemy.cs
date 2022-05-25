using Rooms;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyMovement))]
    public class Enemy : MonoBehaviour
    {
        private ushort _id;

        public ushort Id => _id;

        private Room _room;

        public Room Room => _room;
        
        public void Init(ushort id, Room room)
        {
            _id = id;
            _room = room;
        }
    }
}