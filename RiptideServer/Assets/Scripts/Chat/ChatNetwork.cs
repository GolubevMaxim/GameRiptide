using Authorization;
using RiptideNetworking;

namespace Chat
{
    public class ChatNetwork
    {
        
        [MessageHandler((ushort) ClientToServerId.Chat)]
        private static void ReceiveChatMessage(ushort fromClientId, Message message)
        {
            if (!Player.Players.Dictionary.TryGetValue(fromClientId, out var player)) return;
                player.GetRoom().AddMessageToChat(fromClientId, message.GetString());
        }
    }
}