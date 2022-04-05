using Authorization;
using RiptideNetworking;

namespace Chat
{
    public class ChatNetwork
    {
        
        [MessageHandler((ushort) ClientToServerId.Chat)]
        private static void ReceiveChatMessage(ushort fromClientId, Message message)
        {
            if (!AuthorizationManager.Singleton.Users.TryGetValue(fromClientId, out var user)) return;
        
            var roomIndex = message.GetUShort();
            Rooms.Rooms.GetRoom(roomIndex).AddMessageToChat(fromClientId, message.GetString());
        }
    }
}