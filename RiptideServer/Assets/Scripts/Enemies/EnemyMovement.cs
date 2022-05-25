using Rooms;
using Unity.VisualScripting;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private int attackDistance = 10;
        [SerializeField] private int speed = 5;
        
        private Room _room;
        private Rigidbody2D _rigidbody2D;

        private Vector2 _velocity;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _room = GetComponentInParent<Room>();

        }

        private void LateUpdate()
        {
            _rigidbody2D.velocity = _velocity;
        }

        private void Update()
        {
            Player.Player? targetPlayer = null;
            
            foreach (var player in _room.Players.Values)
            {
                if ((player.transform.position - transform.position).sqrMagnitude < attackDistance)
                {
                    targetPlayer = player;
                } 
            }

            _velocity = Vector2.zero;
            if (targetPlayer != null && (targetPlayer.transform.position - transform.position).sqrMagnitude > 10)
            {
                _velocity = (targetPlayer.transform.position - transform.position) * speed;
            }
        }
    }
}