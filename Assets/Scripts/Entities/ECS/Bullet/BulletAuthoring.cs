using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    public struct BulletData : IComponentData
    {
        public float Damage;
    }

    public class BulletAuthoring : MonoBehaviour
    {
        private class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddComponent(entity, new BulletData());
                AddComponent(entity, new PhysicsVelocity());
                AddComponent(entity, PhysicsMass.CreateDynamic(MassProperties.CreateBox(new float3(0.119999997f, 0.0700000003f, 0.200000003f)), 5f));
            }
        }
    }
}
