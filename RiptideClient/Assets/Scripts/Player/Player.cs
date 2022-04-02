using Client.PlayerPosition;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerUpdater))]
    public class Player : MonoBehaviour
    {
        private ushort _networkID;
        private string _nickname;

        public void Init(ushort networkID, string nickname)
        {
            _networkID = networkID;
            _nickname = nickname;
        }
    }
}
