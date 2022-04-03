using System.Collections.Generic;

namespace Player
{
    public static class Players
    {
        private static readonly Dictionary<ushort, Player> _dictionary = new();
        public static Dictionary<ushort, Player> Dictionary => _dictionary;
                
    }
}