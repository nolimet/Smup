using Entities.ECS.Enemies.Components;
using Entities.Enemies;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Entities.ECS.Enemies.Systems
{
	/// <summary>
	/// Bridge system that converts existing MonoBehaviour Enemy instances to ECS entities
	/// so they can interact with the ECS collision system.
	/// </summary>
	[UpdateInGroup(typeof(InitializationSystemGroup))]
	public partial class HybridEnemyBridgeSystem : SystemBase
	{
		private EntityQuery _enemyQuery;

		protected override void OnCreate()
		{
			_enemyQuery = GetEntityQuery(typeof(EnemyTag));
		}

		protected override void OnUpdate()
		{
			// This system can convert MonoBehaviour enemies to entities on demand
			// For now, it's a placeholder for hybrid functionality
		}

		/// <summary>
		/// Manually create an ECS entity for a MonoBehaviour enemy.
		/// Call this when spawning non-ECS enemies that need to interact with bullets.
		/// </summary>
		public Entity CreateEnemyEntity(Enemy enemy, float colliderRadius = 0.5f)
		{
			var entity = EntityManager.CreateEntity();

			// Add enemy identification
			EntityManager.AddComponentData(entity, new EnemyTag());
			
			// Add enemy data
			EntityManager.AddComponentData(entity, new EnemyData
			{
				health = enemy.Health,
				scrapValue = enemy.scrapValue,
				scrapCloudSize = enemy.scrapCloudSize.x,
				contactDamage = enemy.contactDamage
			});

			// Add transform
			EntityManager.AddComponentData(entity, new LocalTransform
			{
				Position = enemy.transform.position,
				Rotation = quaternion.identity,
				Scale = 1f
			});

			// Add physics velocity
			EntityManager.AddComponentData(entity, new PhysicsVelocity
			{
				Linear = float3.zero,
				Angular = float3.zero
			});

			// Create collider
			var collider = Unity.Physics.SphereCollider.Create(
				new SphereGeometry
				{
					Center = float3.zero,
					Radius = colliderRadius
				},
				new CollisionFilter
				{
					BelongsTo = 1u << 1,      // Enemy layer (bit 1)
					CollidesWith = 1u << 0,   // Bullet layer (bit 0)
					GroupIndex = 0
				}
			);

			EntityManager.AddComponentData(entity, new PhysicsCollider { Value = collider });

			// Add kinematic mass (doesn't move from physics)
			EntityManager.AddComponentData(entity, PhysicsMass.CreateKinematic(MassProperties.UnitSphere));

			return entity;
		}

		/// <summary>
		/// Update an existing entity's position to match the MonoBehaviour enemy.
		/// Call this every frame or in Update for hybrid enemies.
		/// </summary>
		public void SyncEnemyPosition(Entity entity, Vector3 position)
		{
			if (EntityManager.Exists(entity))
			{
				EntityManager.SetComponentData(entity, LocalTransform.FromPosition(position));
			}
		}

		/// <summary>
		/// Update an existing entity's health.
		/// </summary>
		public void SyncEnemyHealth(Entity entity, double health)
		{
			if (EntityManager.Exists(entity))
			{
				var enemyData = EntityManager.GetComponentData<EnemyData>(entity);
				enemyData.health = health;
				EntityManager.SetComponentData(entity, enemyData);
			}
		}

		/// <summary>
		/// Remove the entity when the MonoBehaviour enemy is destroyed.
		/// </summary>
		public void RemoveEnemyEntity(Entity entity)
		{
			if (EntityManager.Exists(entity))
			{
				EntityManager.DestroyEntity(entity);
			}
		}
	}
}

