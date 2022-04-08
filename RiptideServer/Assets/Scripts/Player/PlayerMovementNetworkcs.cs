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
                SendPosition(player.CurrentRoom.RoomId, playerId);
            }
        }

        private static void SendPosition(ushort roomIndex, ushort playerId)
        {
            Rooms.Rooms.Dictionary[roomIndex].Players.TryGetValue(playerId, out var player);
            if (player == null) return;

            var message = Message.Create(MessageSendMode.unreliable, ServerToClientId.PlayerPositionChange);
            
            message.AddUShort(playerId);
            message.AddVector2(player.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}