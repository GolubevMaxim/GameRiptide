using UnityEngine;

namespace Player
{
    public class PlayerUpdater : MonoBehaviour
    {
        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }
}