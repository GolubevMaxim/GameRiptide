using JetBrains.Annotations;
using Player;
using Rooms;
using UnityEngine;

public enum UserState
{
    Unauthorized,
    Menu,
    Game,
}

public class User
{
    public int id;
    public string nickname;

    private UserState _state;
    
    public User(int id, string nickname)
    {
        this.id = id;
        this.nickname = nickname;

        _state = UserState.Menu;
    }

    public void Play()
    {
        _state = UserState.Game;
    }

    public void Leave()
    {
        _state = UserState.Menu;
    }
}
