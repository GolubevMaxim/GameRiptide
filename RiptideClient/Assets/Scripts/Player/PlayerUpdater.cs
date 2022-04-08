using UnityEngine;

namespace Player
{
    public class PlayerUpdater : MonoBehaviour
    {
        private Vector3 _targetPosition = Vector3.zero;
        [SerializeField] private float interpolationCoefficient = 8f;
        public void SetPosition(Vector2 position)
        {
            _targetPosition = position;
        }

        private void Update()
        {
            var direction = _targetPosition - transform.position;
            
            transform.position += direction * Time.deltaTime * interpolationCoefficient;
        }
    }
}