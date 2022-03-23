using System.Collections.Generic;
using RiptideNetworking;

public class RoomChat
{
    private Message _chatBuffer;
    private readonly List<ushort> _chatUserIDs;

    public RoomChat()
    {
        _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
        _chatUserIDs = new List<ushort>();
    }

    public void AddUser(ushort userID)
    {
        _chatUserIDs.Add(userID);
    }

    public void RemoveUser(ushort userID)
    {
        _chatUserIDs.Remove(userID);
    }
    
    public void AddMessageToBuffer(int userID, string str)
    {
        _chatBuffer.AddInt(userID);
        _chatBuffer.AddString(str);
    }

    public void SendMessages()
    {
        foreach (var userId in _chatUserIDs)
            NetworkManager.Singleton.Server.Send(_chatBuffer, userId, false);

        _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
    }
}