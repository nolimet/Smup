using Entities.ECS.ScreenBoundery.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Entities.ECS.ScreenBoundery.Systems
{
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	public partial struct BulletBoundarySystem : ISystem
	{
		private bool _initialized;

		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<ViewBounds>();
			state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
			state.RequireForUpdate<PhysicsWorldSingleton>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			var bounds = SystemAPI.GetSingleton<ViewBounds>().Value;

			var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();
			var ecb = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged);

			foreach (var (transform, entity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<ViewBoundComponent>().WithEntityAccess())
			{
				var pos = transform.ValueRO.Position;

				if (pos.x < bounds.x || pos.x > bounds.y || pos.y < bounds.z || pos.y > bounds.w)
				{
					var comp = SystemAPI.GetComponent<ViewBoundComponent>(entity);
					if (comp.DestroyWhenOutOfView)
					{
						ecb.DestroyEntity(entity);
					}
					else
					{
						ecb.AddComponent<Disabled>(entity);
					}
				}
			}
		}
	}
}
