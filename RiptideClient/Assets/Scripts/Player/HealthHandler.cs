using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public class HealthHandler : MonoBehaviour
    {
        private int _healthMax;
        private int _health;
        [SerializeField] private GameObject _hvTemplate;
        private GameObject _hvObj;
        private HealthVisualiser _healthVisualiser;

        public void setHealth(int health)
        {
            _health = health;
            _healthVisualiser.updateHealth(health);
        }

        public void setHealthMax(int healthMax)
        {
            _healthMax = healthMax;
        }
        public void Init(int healthMax, int health)
        {
            _health = health;
            _healthMax = healthMax;
            _hvObj = Instantiate(_hvTemplate, transform.position, Quaternion.identity);
            _healthVisualiser = _hvObj.GetComponent<HealthVisualiser>();
            _healthVisualiser.Init(transform, _healthMax, _health);
        }

        private void OnDestroy()
        {
            Debug.Log("Player destroyed.");
            Destroy(_hvObj);
        }
    }
}
