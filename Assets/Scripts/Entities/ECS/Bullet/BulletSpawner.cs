using System.Collections.Generic;
using Entities.ECS.Bullet.Components;
using Entities.ECS.Bullet.Enums;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Entities.ECS.Bullet
{
	public class BulletSpawner
	{
		private static BulletSpawner _instance;

		private Dictionary<BulletType, Entity> _bulletLibrary;
		private EntityManager _entityManager;

		private BulletSpawner() => _bulletLibrary = GetPrefabsFromLibrary();

		private Dictionary<BulletType, Entity> GetPrefabsFromLibrary()
		{
			_entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
			var bulletQuery = _entityManager.CreateEntityQuery(typeof(BulletLibraryTag));

			if (bulletQuery.IsEmpty)
			{
				Debug.LogError("No Bullet Library found in the scene!");
				return new Dictionary<BulletType, Entity>();
			}

			Dictionary<BulletType, Entity> library = new();

			var libraryEntity = bulletQuery.GetSingletonEntity();
			var buffer = _entityManager.GetBuffer<BulletPrefabContainer>(libraryEntity);

			for (var i = 0; i < buffer.Length; i++)
				library.Add(buffer[i].Type, buffer[i].Prefab);

			Debug.Log($"Found {library.Count} Bullet Prefabs");
			return library;
		}

		public static void Init()
		{
			_instance ??= new BulletSpawner();
		}

		public static void DeInit()
		{
			_instance = null;
		}

		[BurstCompile]
		public static void Shoot(BulletType type, float3 position, float3 inheritVelocity, float[] angles, float speed, float damage)
		{
			var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
			if (!_instance._bulletLibrary.TryGetValue(type, out var prefab))
			{
				return;
			}

			var count = angles.Length;
			var bullets = new NativeArray<Entity>(count, Allocator.Temp);

			var pooledQuery = em.CreateEntityQuery(
				ComponentType.ReadOnly<BulletTypeShared>(),
				ComponentType.ReadWrite<Disabled>()
			);
			pooledQuery.SetSharedComponentFilter(new BulletTypeShared {Value = type});

			var pooled = pooledQuery.ToEntityArray(Allocator.Temp);
			var reuseCount = math.min(pooled.Length, count);

			for (var i = 0; i < reuseCount; i++)
			{
				var bullet = pooled[i];
				em.RemoveComponent<Disabled>(bullet);
				bullets[i] = bullet;
			}

			pooled.Dispose();
			pooledQuery.ResetFilter();

			var remaining = count - reuseCount;
			if (remaining > 0)
			{
				using var instances = new NativeArray<Entity>(remaining, Allocator.Temp);
				em.Instantiate(prefab, instances);
				for (var i = 0; i < remaining; i++)
					bullets[reuseCount + i] = instances[i];
			}

			for (var i = 0; i < count; i++)
			{
				var angle = angles[i];
				var bullet = bullets[i];
				var velocity = new float3(
					math.cos(math.radians(angle)) * speed,
					math.sin(math.radians(angle)) * speed,
					0
				);

				em.SetComponentData(bullet, new BulletData {Damage = damage});
				em.SetComponentData(bullet, new PhysicsVelocity
				{
					Linear = velocity + inheritVelocity
				});
				em.SetComponentData(bullet, LocalTransform.FromPositionRotation(
					position,
					quaternion.Euler(0, 0, math.radians(angle)))
				);
			}

			bullets.Dispose();
		}
	}
}
