using UnityEngine;

namespace Player
{
    public class LocalPlayerController : MonoBehaviour
    {
        private Vector2 _previousDirection = Vector2.zero;
        private void Update()
        {
            if (NetworkManager.Singleton.Client == null) return;

            Move();
        }
        private Vector2 GetDirectionFromKeyBoard()
        {
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");

            var directionInput = new Vector2(horizontalInput, verticalInput);
            
            return directionInput;
        }
        
        private void Move()
        {
            var movementDirection = GetDirectionFromKeyBoard();
            
            if (_previousDirection != movementDirection)
            {
                _previousDirection = movementDirection;
                PlayerPositionHandler.SendDirection(movementDirection);
            }
        }
    }
}