using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        private Rigidbody2D _rgbd;

        private void Awake()
        {
            _rgbd = GetComponent<Rigidbody2D>();
        }
        public void Move(Vector2 direction)
        {
            _rgbd.velocity = direction * speed;
        }
    }
}