using RiptideNetworking;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour, Killable
    {
        private int _healthMax;
        private int _health;
        private Player _player;
        public int HealthMax => _healthMax;
        public int Health => _health;

        public void Init(int healthMax, int health)
        {
            _healthMax = healthMax;
            _health = _healthMax;
            _player = GetComponent<Player>();
        }

        public void ApplyDamage(int dmg)
        {
            _health -= dmg;
            SendHealthUpdate();
            if (_health < 0)
            {
                _health = 0;
                Die();
            }
        }

        public void ApplyHeal(int heal)
        {
            _health += heal;
            SendHealthUpdate();
            if (_health > _healthMax) _health = _healthMax;
        }

        public void Die()
        {
            Debug.Log($"Here {this.name} should have died.");
        }

        private void SendHealthUpdate()
        {
            var message = Message.Create(MessageSendMode.reliable, ServerToClientId.UpdateHealth);

            message.AddUShort(_player.NetworkId);
            message.AddInt(_health);

            //send room
            foreach (var playerNetworkID in _player.CurrentRoom.Players.Keys)
                NetworkManager.Singleton.Server.Send(message, playerNetworkID, false);
        }
    }
}
