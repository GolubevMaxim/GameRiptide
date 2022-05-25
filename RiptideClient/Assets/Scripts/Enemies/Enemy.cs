using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies
{
    [RequireComponent(typeof(EnemyUpdater))]
    public class Enemy : MonoBehaviour
    {
        private ushort _id;

        public ushort Id => _id;

        public EnemyUpdater enemyUpdater;

        public void Init(ushort id)
        {
            _id = id;
            enemyUpdater = GetComponent<EnemyUpdater>();
        }

        private void OnMouseDown()
        {
            GameEvents.setTargetEvent.Invoke(_id, TargetType.mobe);
        }
    }
}