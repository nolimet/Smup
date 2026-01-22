using Entities.ECS.Bullet.Enums;
using Unity.Entities;

namespace Entities.ECS.Bullet.Components
{
    public struct BulletPrefabContainer : IBufferElementData
    {
        public BulletType Type;
        public Entity Prefab;
    }
}
