using System;
using System.Collections.Generic;
using Entities.ECS.Bullet.Enums;
using Unity.Entities;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    [CreateAssetMenu(menuName = "Smup/Bullet Config", fileName = "BulletConfig")]
    public class BulletConfig : ScriptableObject
    {
        [Serializable]
        public struct BulletEntry
        {
            public BulletType type;
            public Sprite sprite;
            public Material material;
            public Vector2 colliderSize;
            public float mass;

            [LayerField]
            public uint belongsToMask;

            [SerializeField] private LayerMask colliderWithMask;
            public uint CollidesWithMask => (uint)(int)colliderWithMask;
        }

        public List<BulletEntry> entries = new();
    }
}
