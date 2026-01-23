using Unity.Entities;
using UnityEngine;

namespace Entities.ECS.Bullet.Components
{
    public class BulletRenderSource : IComponentData
    {
        public Sprite Sprite;
        public Material Material;
    }
}
