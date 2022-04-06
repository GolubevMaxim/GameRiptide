using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class Rooms : MonoBehaviour
    {
        private static readonly Dictionary<ushort, Room> _dictionary = new();

        public static Dictionary<ushort, Room> Dictionary => _dictionary;
        
        public static void Add(ushort roomId, Room room)
        { 
            _dictionary[roomId] = room;
        }

        public static Room GetRoom(ushort id)
        {
            return id < _dictionary.Count ? _dictionary[id] : null;
        }
    }
}