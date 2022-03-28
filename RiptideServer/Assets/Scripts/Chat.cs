using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Chat
{
    private Message _chatBuffer;
    private readonly Dictionary<ushort, Player> _chatUsers;

    public Chat()
    {
        _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
        _chatUsers = new Dictionary<ushort, Player>();
    }

    public Chat(Dictionary<ushort, Player> players)
    {
        _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
        _chatUsers = players;
    }

    public void AddMessageToBuffer(ushort playerNetworkID, string str)
    {
        _chatBuffer.AddUShort(playerNetworkID);
        _chatBuffer.AddString(str);
    }

    public void SendMessages()
    {
        foreach (var userId in _chatUsers.Keys)
            NetworkManager.Singleton.Server.Send(_chatBuffer, userId, false);

        _chatBuffer.Release();
        _chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.Chat);
    }
}
