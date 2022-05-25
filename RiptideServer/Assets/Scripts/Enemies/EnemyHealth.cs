using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyHealth : MonoBehaviour, Killable
    {
        private int _healthMax;
        private int _health;

        private Enemy _enemy;

        private void Start()
        {
            _enemy = gameObject.GetComponentInParent<Enemy>();
        }

        public void ApplyDamage(int damage)
        {
            if (damage < 0)
                throw new ArgumentException("damage must be positive");

            _health -= damage;
         
            if (_health <= 0)
            {
                _health = 0;
                Die();
            }
        }

        public void ApplyHeal(int heal)
        {
            if (heal < 0)
                throw new ArgumentException("heal must be positive");
            
            _health += heal;
            if (_health > _healthMax) _health = _healthMax;
        }

        public void Die()
        {
            Destroy(gameObject);
            Debug.Log($"Here {this.name} should have died.");
        }
    }
}