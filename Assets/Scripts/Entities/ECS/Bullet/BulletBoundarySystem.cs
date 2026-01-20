using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public partial struct BulletBoundarySystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
        }

        private float4 _bounds; // x: minX, y: maxX, z: minY, w: maxY

        public void OnUpdate(ref SystemState state)
        {
            // We calculate bounds once per frame. 
            // In a real shmup, you'd likely use a fixed game area or the camera frustum.
            var cam = Camera.main;
            if (cam == null) return;

            // Get screen bounds in world space (with a small buffer so they don't pop out)
            var buffer = 1.0f;
            var bottomLeft = cam.ViewportToWorldPoint(new Vector3(0, 0, cam.nearClipPlane));
            var topRight = cam.ViewportToWorldPoint(new Vector3(1, 1, cam.nearClipPlane));

            _bounds = new float4(bottomLeft.x - buffer, topRight.x + buffer, bottomLeft.y - buffer, topRight.y + buffer);

            var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
            var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

            // Schedule the job to check positions
            foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<BulletData>().WithEntityAccess())
            {
                var pos = transform.ValueRO.Position;

                if (pos.x < _bounds.x || pos.x > _bounds.y || pos.y < _bounds.z || pos.y > _bounds.w) ecb.DestroyEntity(entity);
            }
        }
    }
}
