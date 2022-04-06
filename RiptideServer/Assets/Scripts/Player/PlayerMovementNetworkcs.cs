using RiptideNetworking;
using UnityEngine;

namespace Player
{
    public class PlayerMovementNetwork : MonoBehaviour
    {
        [MessageHandler((ushort) ClientToServerId.DirectionInput)]
        private static void GetPlayerMovementMessage(ushort playerId, Message message)
        {
            var roomIndex = message.GetUShort();
            var moveDirection = message.GetVector2();
            
            Rooms.Rooms.List[roomIndex].Players.TryGetValue(playerId, out var player);
            
            if (player == null) return;
            
            player.GetComponent<PlayerMovement>().Move(moveDirection);
            SendPosition(roomIndex, playerId);
        }

        private static void SendPosition(ushort roomIndex, ushort playerId)
        {
            Rooms.Rooms.List[roomIndex].Players.TryGetValue(playerId, out var player);
            if (player == null) return;

            var message = Message.Create(MessageSendMode.unreliable, ServerToClientId.PlayerPositionChange);
            
            message.AddUShort(playerId);
            message.AddVector2(player.transform.position);
            
            NetworkManager.Singleton.Server.SendToAll(message);
        }
    }
}