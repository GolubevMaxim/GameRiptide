using RiptideNetworking;
using UnityEngine;

namespace Chat
{
    public class ChatNetwork : MonoBehaviour
    {
        public static Chat CurrentChat;
        
        private static ChatNetwork _singleton;

        public static ChatNetwork Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(Chat)} instance already exists, destroying duplicate.");
                    Destroy(value);
                }
            }
        }

        private void Awake()
        {
            _singleton = this;
        }
        
        public static void SendChatMessage(string chatMessage)
        {
            var message = Message.Create(MessageSendMode.reliable, ClientToServerId.Chat);

            //message.AddUShort(Room.RoomNetwork.CurrentRoom.RoomId);
            message.AddString(chatMessage);

            NetworkManager.Singleton.Client.Send(message);
        }

        [MessageHandler((ushort) ServerToClientId.Chat)]
        private static void ReceiveChatMessages(Message message)
        {
            while (message.UnreadLength > 0)
            {
                CurrentChat.ChatLogAdd(message.GetUShort(), message.GetString());
            }
        }
    }
}