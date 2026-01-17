using Entities.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    public class BulletSpawner
    {
        private EntityQuery _bulletQuery;
        private static BulletSpawner _instance;

        private BulletSpawner()
        {
            var entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            _bulletQuery = entityManager.CreateEntityQuery(typeof(BulletTag));
        }

        public static void Init()
        {
            _instance ??= new BulletSpawner();
        }

        public static void DeInit()
        {
            _instance = null;
        }

        public static void Shoot(BulletGeneric.BulletType type, float3 position, float3 inheritVelocity, float angle, float speed, float damage)
        {
            var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;

            // You could cache the library entity here to make it even faster
            var prefab = _instance.GetPrefabFromLibrary(type);

            if (prefab != Entity.Null)
            {
                var bullet = em.Instantiate(prefab);
                var velocity = new float3(
                    math.cos(math.radians(angle)) * speed,
                    math.sin(math.radians(angle)) * speed,
                    0
                );

                em.SetComponentData(bullet, new BulletData { Damage = damage });
                em.SetComponentData(bullet, new PhysicsVelocity
                {
                    Linear = velocity + inheritVelocity
                });
                em.SetComponentData(bullet, LocalTransform.FromPositionRotation(
                    position,
                    quaternion.Euler(0, 0, math.radians(angle)))
                );
            }
        }

        private Entity GetPrefabFromLibrary(BulletGeneric.BulletType type)
        {
            var entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            _bulletQuery = entityManager.CreateEntityQuery(typeof(BulletTag));

            if (_bulletQuery.IsEmpty)
            {
                Debug.LogError("No Bullet Library found in the scene!");
                return Entity.Null;
            }

            var libraryEntity = _bulletQuery.GetSingletonEntity();

            // 3. Get the Buffer (our list of prefabs)
            var buffer = entityManager.GetBuffer<BulletVisualElement>(libraryEntity);

            // 4. Look for the matching enum
            for (var i = 0; i < buffer.Length; i++)
                if (buffer[i].Type == type)
                    return buffer[i].Prefab;

            Debug.LogWarning($"Bullet type {type} not found in library!");
            return Entity.Null;
        }
    }
}
