using UnityEngine;

namespace Teleport
{
    public class TeleportIn : MonoBehaviour
    {
        [SerializeField] private TeleportOut _teleportOut;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent<Player.Player>(out var player))
            {
                player.CurrentRoom.TryRemovePlayer(player.NetworkId);
                _teleportOut.TeleportPlayer(player);
            }
        }
    }
}