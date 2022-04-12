using System.Collections.Generic;
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
            
            Flip(direction);

            transform.position = direction * Time.deltaTime * interpolationCoefficient + LayerHandler.getCorrectedZ(transform.position);
        }

        private void Flip(Vector2 moveDirection)
        {
            transform.rotation = Quaternion.Euler
            (
                new Vector3
                (
                    0, Mathf.Abs(Vector2.SignedAngle(moveDirection, Vector2.right)) < 90 ? 0 : 180, 0
                )
            );
        }
    }
}