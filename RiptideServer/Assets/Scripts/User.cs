using JetBrains.Annotations;
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
    [CanBeNull] public Room room;
    public UserState state;
    
    public User(int id, string nickname, Room room = null)
    {
        this.id = id;
        this.nickname = nickname;
        this.room = room;

        state = UserState.Menu;
    }

    public Room GetRoom()
    {
        if (state == UserState.Game)
            return room;
        
        Debug.LogWarning($"Trying to get room from player not in game. User id = {id}");
        return null;
    }

    public bool RemoveFromGame(ushort userNetworkId)
    {
        if (room == null)
            return false;
        
        
        room.RemovePlayer(userNetworkId);
        
        state = UserState.Menu;
        
        return true;
    }
}
