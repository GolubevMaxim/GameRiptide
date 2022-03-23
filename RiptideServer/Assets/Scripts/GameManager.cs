using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _singleton;

    public static GameManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
                _singleton = value;
            else if (_singleton != value)
            {
                Debug.Log($"{nameof(GameManager)} instance already exists, destroying duplicate.");
                Destroy(value);
            }
        }
    }

    public Dictionary<ushort, User> Users;
    public Dictionary<ushort, User> UsersInMenu;
    public Room[] Rooms;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Users = new Dictionary<ushort, User>();
        UsersInMenu = new Dictionary<ushort, User>();
        Rooms = new Room[1];
        Rooms[0] = new Room();
    }

    public void RemoveUser(ushort userNetworkId)
    {
        if (Users.TryGetValue(userNetworkId, out var user))
        {
            user.RemoveFromGame(userNetworkId);
            UsersInMenu.Remove(userNetworkId);
            Users.Remove(userNetworkId);
        }
    }

    public bool AddUserToGame(ushort userNetworkId)
    {
        if (UsersInMenu.TryGetValue(userNetworkId, out var user))
        {
            var room = 0;
            
            GetRoom(room).AddPlayer(userNetworkId, user);
            UsersInMenu.Remove(userNetworkId);
            
            return true;
        }
        return false;
    }

    public void RemoveUserFromGame(ushort userNetworkId)
    {
        if (Users.TryGetValue(userNetworkId, out var user))
        {
            if (!user.RemoveFromGame(userNetworkId))
            {
                Debug.LogWarning($"Failed to remove player {user.id} from room {user.room}: the player doesn't exisit in the room.");
                return;
            }
            UsersInMenu.Add(userNetworkId, user);
        }
    }

    private Room GetRoom(int roomNum)
    {
        if (roomNum < 0 || roomNum >= Rooms.Length) return null;
        return Rooms[roomNum];
    }
}
