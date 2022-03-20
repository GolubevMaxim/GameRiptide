using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UserState
{
    unauthorized,
    menu,
    game
}

public class User
{
    public int id;
    public string nickname;
    public int room;
    public UserState state;
    public User(int id, string nickname, int lastRoom)
    {
        this.id = id;
        this.nickname = nickname;
        state = UserState.menu;
        if (lastRoom < 0 || lastRoom >= GameManager.Singleton.rooms.Length) room = 0;
        else room = lastRoom;
    }

    public Room getRoom()
    {
        if (state != UserState.game)
        {
            Debug.LogWarning($"Trying to get room from player not in game. User id = {id}");
            return null;
        }
        if(room < 0 || room >= GameManager.Singleton.rooms.Length)
        {
            Debug.LogWarning($"Room number out of array boundaries in user {id}: room = {room}");
            return null;
        }
        return GameManager.Singleton.rooms[room];
    }

    public bool removeFromGame(ushort userNetworkId)
    {
        Room room = getRoom();
        if(room == null)
        {
            return false;
        }
        room.RemovePlayer(userNetworkId);
        state = UserState.menu;
        return true;
    }
}
