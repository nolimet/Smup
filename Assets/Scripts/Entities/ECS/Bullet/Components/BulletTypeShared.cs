using Entities.ECS.Bullet.Enums;
using Unity.Entities;

namespace Entities.ECS.Bullet.Components
{
    public struct BulletTypeShared : ISharedComponentData
    {
        public BulletType Value;
    }
}
