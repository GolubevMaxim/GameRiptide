using UnityEngine;

namespace Player
{ 
    public class Player : MonoBehaviour
    {
        private ushort _networkID;
        private string _nickname;
        
        public HealthHandler healthHandler
        {
            get; private set;
        }
        public PlayerUpdater playerUpdater {
            get; private set;
        }

        public void Init(ushort networkID, string nickname, int healthMax, int health)
        {
            _networkID = networkID;
            _nickname = nickname;
            playerUpdater = GetComponent<PlayerUpdater>();
            healthHandler = GetComponent<HealthHandler>();
            healthHandler.Init(healthMax, health);
        }

        public void setHealth(int health)
        {
            healthHandler.setHealth(health);
        }

        public void setHealthMax(int healthMax)
        {
            healthHandler.setHealthMax(healthMax);
        }

        private void OnMouseDown()
        {
            GameEvents.setTargetEvent.Invoke(_networkID, TargetType.player);
        }
    }
}
