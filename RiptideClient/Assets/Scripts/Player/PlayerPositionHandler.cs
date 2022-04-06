using RiptideNetworking;
using UnityEngine;

namespace Player
{
    public static class PlayerPositionHandler
    {
        public static void SendDirection(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.unreliable, ClientToServerId.DirectionInput);

            message.AddUShort(0);
            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        
        [MessageHandler((ushort) ServerToClientId.PlayerPositionChange)]
        private static void GetNewPositionMessage(Message message)
        {
            var playerId = message.GetUShort();
            var position = message.GetVector2();

            Players.Dictionary.TryGetValue(playerId, out var player);
            if (player == null) return;

            player.playerUpdater.SetPosition(position);
        }
    }
}