using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct BulletBoundarySystem : ISystem
    {
        private float4 _bounds;
        private bool _initialized;

        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            if (!_initialized)
            {
                var cam = Camera.main;
                if (cam == null) return;

                var bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
                var topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

                const float buffer = 1.0f;
                _bounds = new float4(bottomLeft.x - buffer, topRight.x + buffer, bottomLeft.y - buffer, topRight.y + buffer);
                _initialized = true;
            }

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BulletData>().WithEntityAccess())
            {
                var pos = transform.ValueRO.Position;

                if (pos.x < _bounds.x || pos.x > _bounds.y || pos.y < _bounds.z || pos.y > _bounds.w) ecb.DestroyEntity(entity);
            }
        }
    }
}
