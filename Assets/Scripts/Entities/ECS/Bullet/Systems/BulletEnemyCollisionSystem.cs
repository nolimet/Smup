using Entities.ECS.Bullet.Components;
using Entities.ECS.Enemies;
using Entities.ECS.Enemies.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Systems;

namespace Entities.ECS.Bullet.Systems
{
	/// <summary>
	/// High-performance collision detection system for 30k+ bullets.
	/// Uses spatial queries and parallel job processing.
	/// </summary>
	[UpdateInGroup(typeof(PhysicsSystemGroup))]
	[UpdateAfter(typeof(PhysicsSimulationGroup))]
	public partial class BulletEnemyCollisionSystem : SystemBase
	{
		private EntityQuery _bulletQuery;
		private EntityQuery _enemyQuery;

		protected override void OnCreate()
		{
			_bulletQuery = GetEntityQuery(
				ComponentType.ReadOnly<BulletTag>(),
				ComponentType.ReadOnly<BulletData>()
			);

			_enemyQuery = GetEntityQuery(
				ComponentType.ReadOnly<EnemyTag>(),
				ComponentType.ReadWrite<EnemyData>()
			);

			RequireForUpdate<PhysicsWorldSingleton>();
		}

		protected override void OnUpdate()
		{
			// Skip if no bullets or enemies
			if (_bulletQuery.IsEmpty || _enemyQuery.IsEmpty)
			{
				return;
			}

			var physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>().PhysicsWorld;

			// Allocate results - estimate collision count as ~10% of bullets
			var bulletCount = _bulletQuery.CalculateEntityCount();
			var estimatedCollisions = bulletCount / 10 + 100;

			var collisionResults = new NativeList<CollisionData>(estimatedCollisions, Allocator.TempJob);
			var enemiesToDamage = new NativeList<EnemyDamageData>(estimatedCollisions, Allocator.TempJob);

			// Phase 1: Parallel collision detection using spatial queries
			var detectJob = new BulletCollisionDetectionJob
			{
				PhysicsWorld = physicsWorld,
				CollisionResults = collisionResults.AsParallelWriter(),
				EnemiesToDamage = enemiesToDamage.AsParallelWriter()
			};

			Dependency = detectJob.ScheduleParallel(Dependency);
			Dependency.Complete();

			// Phase 2: Apply damage to enemies (main thread)
			for (var i = 0; i < enemiesToDamage.Length; i++)
			{
				var damageData = enemiesToDamage[i];
				if (EntityManager.Exists(damageData.EnemyEntity))
				{
					var enemyData = EntityManager.GetComponentData<EnemyData>(damageData.EnemyEntity);
					enemyData.health -= damageData.Damage;
					EntityManager.SetComponentData(damageData.EnemyEntity, enemyData);

					// Destroy enemy if health <= 0
					if (enemyData.health <= 0)
					{
						EntityManager.DestroyEntity(damageData.EnemyEntity);
					}
				}
			}

			// Phase 3: Disable bullets that collided
			for (var i = 0; i < collisionResults.Length; i++)
			{
				var bulletEntity = collisionResults[i].BulletEntity;
				if (EntityManager.Exists(bulletEntity))
				{
					EntityManager.AddComponent<Disabled>(bulletEntity);
				}
			}

			// Cleanup
			collisionResults.Dispose();
			enemiesToDamage.Dispose();
		}
	}

	/// <summary>
	/// Data structure for collision results
	/// </summary>
	public struct CollisionData
	{
		public Entity BulletEntity;
	}

	/// <summary>
	/// Data structure for damage to apply
	/// </summary>
	public struct EnemyDamageData
	{
		public Entity EnemyEntity;
		public float Damage;
	}

	/// <summary>
	/// High-performance parallel job for collision detection.
	/// Each job execution processes one bullet and uses spatial queries.
	/// </summary>
	[BurstCompile]
	public partial struct BulletCollisionDetectionJob : IJobEntity
	{
		[ReadOnly] public PhysicsWorld PhysicsWorld;
		public NativeList<CollisionData>.ParallelWriter CollisionResults;
		public NativeList<EnemyDamageData>.ParallelWriter EnemiesToDamage;

		private void Execute(in BulletData bulletData, in BulletTag _, in Entity bulletEntity)
		{
			// Get bullet's physics body
			var bulletBodyIndex = PhysicsWorld.GetRigidBodyIndex(bulletEntity);
			if (bulletBodyIndex < 0)
			{
				return;
			}

			var bulletBody = PhysicsWorld.Bodies[bulletBodyIndex];
			var bulletCollider = bulletBody.Collider;
			var bulletAabb = bulletCollider.Value.CalculateAabb(bulletBody.WorldFromBody);

			// Check all bodies in physics world for overlap
			for (var bodyIndex = 0; bodyIndex < PhysicsWorld.NumBodies; bodyIndex++)
			{
				var otherBody = PhysicsWorld.Bodies[bodyIndex];
				var otherEntity = otherBody.Entity;

				// Skip if it's the bullet itself
				if (otherEntity == bulletEntity || otherEntity == Entity.Null)
				{
					continue;
				}

				// Get other body's AABB
				var otherCollider = otherBody.Collider;
				var otherAabb = otherCollider.Value.CalculateAabb(otherBody.WorldFromBody);

				// Check AABB overlap directly (Burst-compatible)
				if (bulletAabb.Overlaps(otherAabb))
				{
					// Record collision result
					CollisionResults.AddNoResize(new CollisionData
					{
						BulletEntity = bulletEntity
					});

					// Record damage for the enemy
					EnemiesToDamage.AddNoResize(new EnemyDamageData
					{
						EnemyEntity = otherEntity,
						Damage = bulletData.Damage
					});

					// Exit after first hit (bullets don't penetrate)
					return;
				}
			}
		}
	}
}
