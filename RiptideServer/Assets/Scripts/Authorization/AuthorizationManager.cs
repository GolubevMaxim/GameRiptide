using System.Collections.Generic;
using Rooms;
using UnityEngine;

namespace Authorization
{
    public class AuthorizationManager : MonoBehaviour
    {
        private static AuthorizationManager _singleton;

        public static AuthorizationManager Singleton
        {
            get => _singleton;
            private set
            {
                if (_singleton == null)
                    _singleton = value;
                else if (_singleton != value)
                {
                    Debug.Log($"{nameof(AuthorizationManager)} instance already exists, destroying duplicate.");
                    Destroy(value);
                }
            }
        }

        public Dictionary<ushort, User> Users;
        public Dictionary<ushort, User> UsersInMenu;

        private void Awake()
        {
            Singleton = this;
        }

        private void Start()
        {
            Users = new Dictionary<ushort, User>();
            UsersInMenu = new Dictionary<ushort, User>();
        }

        /*
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
                var room = GetRoom(0);

                var player = new Player.Player(userNetworkId, user, room);
                room.AddPlayer(userNetworkId, player);
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
        }*/
    }
}