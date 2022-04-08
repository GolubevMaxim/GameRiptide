using UnityEngine;

namespace Player
{
    public class PlayerUpdater : MonoBehaviour
    {
        private Vector3 _position = Vector3.zero;
        [SerializeField] private float interpolationCoef = 8f;
        [SerializeField] private GameObject realPosition = null;
        public void SetPosition(Vector3 position)
        {
            _position = position;
            if(realPosition != null)
            {
                realPosition.transform.position = _position;
            }
        }

        private void Update()
        {
            var velocity = _position - transform.position;
            transform.position += velocity * Time.deltaTime * interpolationCoef;
        }
    }
}