using UnityEngine;

namespace Teleport
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private ushort _targetRoomIndex;
        [SerializeField] private GameObject _teleportOut;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.TryGetComponent<Player.Player>(out var player))
            {
                Rooms.Rooms.List[_targetRoomIndex].SpawnPlayer
                    (
                        player.NetworkId,
                        _teleportOut.transform.position,
                        player.NickName
                    );
                
                Destroy(player.gameObject);
            }
        }
    }
}