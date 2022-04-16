using RiptideNetworking;
using UnityEngine;

namespace Player
{
    public class PlayerMovementNetwork : MonoBehaviour
    {
        [MessageHandler((ushort) ClientToServerId.DirectionInput)]
        private static void GetPlayerMovementMessage(ushort playerId, Message message)
        {
            var moveDirection = message.GetVector2();

            if (Players.Dictionary.TryGetValue(playerId, out var player))
            {
                if (moveDirection.magnitude > 1) moveDirection = moveDirection.normalized;
                player.GetComponent<PlayerMovement>().Move(moveDirection);
            }
        }
    }
}