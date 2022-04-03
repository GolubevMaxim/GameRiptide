using System.Collections.Generic;
using UnityEngine;

namespace Rooms
{
    public class Rooms : MonoBehaviour
    {
        public static List<Room> List = new();

        public static void Add(Room room)
        {
            List.Add(room);
        }
    }
}