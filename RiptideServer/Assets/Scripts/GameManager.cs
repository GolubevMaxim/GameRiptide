using System.Collections;
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

    public Dictionary<ushort, User> users;
    public Dictionary<ushort, User> usersInMenu;
    public Room[] rooms;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        users = new Dictionary<ushort, User>();
        usersInMenu = new Dictionary<ushort, User>();
        rooms = new Room[1];
        rooms[0] = new Room();
    }

    public void RemoveUser(ushort userNetworkId)
    {
        User user;
        if (users.TryGetValue(userNetworkId, out user))
        {
            user.removeFromGame(userNetworkId);
            usersInMenu.Remove(userNetworkId);
            users.Remove(userNetworkId);
        }
    }

    public bool AddUserToGame(ushort userNetworkId)
    {
        User user;
        if (usersInMenu.TryGetValue(userNetworkId, out user))
        {
            int room = 0;
            GetRoom(room).AddPlayer(userNetworkId, user);
            usersInMenu.Remove(userNetworkId);
            return true;
        }
        return false;
    }

    public void RemoveUserFromGame(ushort userNetworkId)
    {
        User user;
        if (users.TryGetValue(userNetworkId, out user))
        {
            if (!user.removeFromGame(userNetworkId))
            {
                Debug.LogWarning($"Failed to remove player {user.id} from room {user.room}: the player doesn't exisit in the room.");
                return;
            }
            usersInMenu.Add(userNetworkId, user);
        }
    }

    private Room GetRoom(int roomNum)
    {
        if (roomNum < 0 || roomNum >= rooms.Length) return null;
        return rooms[roomNum];
    }
}
