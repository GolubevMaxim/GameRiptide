using UnityEngine;

namespace Client.PlayerPosition
{
    public class PlayerUpdater : MonoBehaviour
    {
        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }
    }
}