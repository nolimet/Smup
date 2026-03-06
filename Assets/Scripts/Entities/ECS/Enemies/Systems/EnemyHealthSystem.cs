using Entities.ECS.Enemies.Components;
using Unity.Burst;
using Unity.Entities;

namespace Entities.ECS.Enemies.Systems
{
	/// <summary>
	/// System that monitors enemy health and marks dead enemies for cleanup.
	/// Works alongside the collision system to handle enemy destruction.
	/// </summary>
	[UpdateInGroup(typeof(SimulationSystemGroup))]
	[UpdateAfter(typeof(Unity.Physics.Systems.PhysicsSystemGroup))]
	public partial struct EnemyHealthSystem : ISystem
	{
		public void OnCreate(ref SystemState state)
		{
			state.RequireForUpdate<EnemyTag>();
		}

		[BurstCompile]
		public void OnUpdate(ref SystemState state)
		{
			// Process enemies that have been damaged by collision system
			foreach (var (enemyData, entity) in 
			         SystemAPI.Query<RefRO<EnemyData>>()
				         .WithAll<EnemyTag>()
				         .WithEntityAccess())
			{
				// Enemy is dead - collision system already destroyed it
				// This is just for any additional cleanup if needed
				if (enemyData.ValueRO.health <= 0)
				{
					// Additional cleanup logic can go here
					// For now, collision system handles destruction
				}
			}
		}
	}
}

