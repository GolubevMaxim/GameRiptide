using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RiptideNetworking;

public class Player
{
    public ushort networkID;
    public Room room;
    public User user;
    public Vector2 position;

    public Player(ushort networkID, User user, Room room)
    {
        this.networkID = networkID;
        this.user = user;
        this.room = room;
        position = Vector2.zero;

    }

    public void EnterRoom(Room room)
    {
        user.state = UserState.Game;
        this.room = room;
    }
}
