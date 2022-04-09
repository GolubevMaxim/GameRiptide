using System;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody2D _rigidbody2D;

        private Vector2 _velocity;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }
        public void Move(Vector2 direction)
        {
            _velocity = direction * speed;
        }

        private void LateUpdate()
        {
            _rigidbody2D.velocity = _velocity;
        }
    }
}