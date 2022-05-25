﻿using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(EnemyHealth))]
    public class Enemy : MonoBehaviour
    {
        private ushort _id;

        public ushort Id => _id;
        
        public void Init(ushort id)
        {
            _id = id;
        }
    }
}