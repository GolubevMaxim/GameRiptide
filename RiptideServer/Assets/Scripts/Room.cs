using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Room
{
    public Dictionary<ushort, User> users;
    public bool bufferEmpty;
    public Message chatBuffer;

    public Room()
    {
        users = new Dictionary<ushort, User>();
        chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.chat);
        bufferEmpty = true;
    }

    public void AddPlayer(ushort id, User user)
    {
        users.Add(id, user);
        Debug.Log($"Room accepted the player {id}");
        user.state = UserState.game;
    }

    public bool RemovePlayer(ushort id)
    {
        if (users.Remove(id))
        {
            Debug.Log($"Room kicked out the player {id}.");
            return true;
        }
        Debug.LogWarning($"Failed to remove player {id}.");
        return false;
    }

    public void AddMessageToBuffer(int userId, string str)
    {
        chatBuffer.AddInt(userId);
        chatBuffer.AddString(str);
    }

    public void SendMessages()
    {
        if (bufferEmpty) return;
        foreach (ushort userId in users.Keys)
        {
            NetworkManager.Singleton.server.Send(chatBuffer, userId, false);
        }
        bufferEmpty = true;
        chatBuffer.Release();
        chatBuffer = Message.Create(MessageSendMode.reliable, ServerToClientId.chat);
    }
}
