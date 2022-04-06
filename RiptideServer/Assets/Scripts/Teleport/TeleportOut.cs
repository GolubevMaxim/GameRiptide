using Rooms;
using UnityEngine;

namespace Teleport
{
    public class TeleportOut : MonoBehaviour
    {
        [SerializeField] private Room _room;
        
        public void TeleportPlayer(Player.Player player)
        {
            _room.AddPlayer(player, transform.position);
        }
    }
}