using RiptideNetworking;
using UnityEngine;

namespace Player
{
    public static class PlayerPositionHandler
    {
        public static void SendDirection(Vector2 direction)
        {
            var message = Message.Create(MessageSendMode.unreliable, ClientToServerId.DirectionInput);

            message.AddVector2(direction);
            
            NetworkManager.Singleton.Client.Send(message);
        }

        
        [MessageHandler((ushort) ServerToClientId.PlayerPositionChange)]
        private static void GetNewPositionMessage(Message message)
        {
            while(message.UnreadLength > 0)
            {
                var playerId = message.GetUShort();
                var position = message.GetVector2();

                Players.Dictionary.TryGetValue(playerId, out var player);
                if (player == null) return;

                player.playerUpdater.SetPosition(position);
            }
            
        }
    }
}