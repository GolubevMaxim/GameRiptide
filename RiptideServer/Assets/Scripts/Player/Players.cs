using System.Collections.Generic;

namespace Player
{
    public static class Players
    {
        private static Dictionary<ushort, Player> _dictionary = new();

        public static Dictionary<ushort, Player> Dictionary => _dictionary;

        public static void Add(Player player)
        {
            _dictionary.Add(player.NetworkId, player);
        }
    }
}