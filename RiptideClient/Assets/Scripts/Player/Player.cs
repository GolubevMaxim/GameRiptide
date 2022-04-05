using UnityEngine;
using Client.PlayerPosition;

namespace Player
{ 
    public class Player : MonoBehaviour
    {
        private ushort _networkID;
        private string _nickname;
        public PlayerUpdater playerUpdater {
            get; private set;
        }

        public void Init(ushort networkID, string nickname)
        {
            _networkID = networkID;
            _nickname = nickname;
            playerUpdater = GetComponent<PlayerUpdater>();
        }
    }
}
