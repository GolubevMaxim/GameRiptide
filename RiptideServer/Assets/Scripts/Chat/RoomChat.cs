using System.Collections.Generic;
using RiptideNetworking;

namespace Chat
{ 
    public class RoomChat
    {
        private Message _chatBuffer;
        private readonly List<ushort> _chatUserIDs;

        public RoomChat()
        {
            _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
            _chatUserIDs = new List<ushort>();
        }

        public void AddPlayer(ushort playerID)
        {
            _chatUserIDs.Add(playerID);
        }

        public void RemovePlayer(ushort playerID)
        {
            _chatUserIDs.Remove(playerID);
        }

        public void AddMessageToBuffer(ushort playerID, string str)
        {
            _chatBuffer.AddUShort(playerID);
            _chatBuffer.AddString(str);
        }

        public void SendMessages()
        {
            foreach (var userId in _chatUserIDs)
                NetworkManager.Singleton.Server.Send(_chatBuffer, userId, false);

            _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
        }
    }
}
