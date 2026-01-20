using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    public struct BulletData : IComponentData
    {
        public float Damage;
        public uint TargetLayerMask;
    }

    public class BulletAuthoring : MonoBehaviour
    {
        private class Baker : Baker<BulletAuthoring>
        {
            public override void Bake(BulletAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);

                // This makes the Prefab compatible with your Spawner's SetComponentData calls
                AddComponent(entity, new BulletData());

                // IMPORTANT: If you want to use Unity Physics, 
                // you must also have a PhysicsBody component on the GameObject
                // and this component will be baked automatically. 
                // But we add the data component here for logic.
                AddComponent(entity, new PhysicsVelocity());
                AddComponent(entity, PhysicsMass.CreateDynamic(MassProperties.CreateBox(new float3(0.119999997f, 0.0700000003f, 0.200000003f)), 5f));
            }
        }
    }
}
