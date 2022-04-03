using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        
        private bool _inDash;
        private float _previousTime;
        public void Move(Vector2 direction)
        {
            if (_inDash)
            {
                _previousTime = Time.time;
                return;
            }

            var previousTime = _previousTime;

            var deltaTime = Time.time - previousTime;
            transform.Translate(direction.normalized * deltaTime * speed);

            _previousTime = Time.time;
        }
    }
}