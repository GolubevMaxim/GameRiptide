using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class HealthHandler : MonoBehaviour
    {
        private int _healthMax;
        private int _health;
        private HealthVisualiser healthVisualiser;

        public void setHealth(int health)
        {
            _health = health;
        }

        public void setHealthMax(int healthMax)
        {
            _healthMax = healthMax;
        }
        public void Init(int healthMax, int health)
        {
            _health = health;
            _healthMax = healthMax;
            healthVisualiser = GetComponentInChildren<HealthVisualiser>();
            healthVisualiser.Init(_healthMax, _health);
        }
    }
}
