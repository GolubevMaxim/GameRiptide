using System.Collections.Generic;
using UnityEngine;

public class Room
{
    private readonly Dictionary<ushort, User> _users;
    private readonly RoomChat _roomChat;

    public Room()
    {
        _users = new Dictionary<ushort, User>();
        _roomChat = new RoomChat();
    }

    public void AddMessageToChat(int userID, string chatMessage)
    {
        _roomChat.AddMessageToBuffer(userID, chatMessage);
    }

    public void SendChat()
    {
        _roomChat.SendMessages();
    }

    public void AddPlayer(ushort id, User user)
    {
        _roomChat.AddUser(id);
        _users.Add(id, user);
        
        Debug.Log($"Room accepted the player {id}");
        
        user.state = UserState.Game;
    }

    public bool RemovePlayer(ushort id)
    {
        if (_users.Remove(id))
        {
            _roomChat.RemoveUser(id);
            
            Debug.Log($"Room kicked out the player {id}.");
            return true;
        }
        
        Debug.LogWarning($"Failed to remove player {id}.");
        return false;
    }
}
