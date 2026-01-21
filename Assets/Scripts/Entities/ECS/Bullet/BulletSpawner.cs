using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    public enum BulletType
    {
        Fragment,
        Bullet
    }

    public class BulletSpawner
    {
        private static BulletSpawner _instance;

        private Dictionary<BulletType, Entity> _bulletLibrary;
        private EntityManager _entityManager;

        private BulletSpawner() => _bulletLibrary = GetPrefabsFromLibrary();

        private Dictionary<BulletType, Entity> GetPrefabsFromLibrary()
        {
            _entityManager = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            var _bulletQuery = _entityManager.CreateEntityQuery(typeof(BulletLibaryTag));

            if (_bulletQuery.IsEmpty)
            {
                Debug.LogError("No Bullet Library found in the scene!");
                return new Dictionary<BulletType, Entity>();
            }

            Dictionary<BulletType, Entity> library = new();

            var libraryEntity = _bulletQuery.GetSingletonEntity();
            var buffer = _entityManager.GetBuffer<BulletVisualElement>(libraryEntity);

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

        public static void Shoot(BulletType type, float3 position, float3 inheritVelocity, float[] angles, float speed, float damage)
        {
            var em = Unity.Entities.World.DefaultGameObjectInjectionWorld.EntityManager;
            if (!_instance._bulletLibrary.TryGetValue(type, out var prefab)) return;

            for (var i = 0; i < angles.Length; i++)
            {
                var angle = angles[i];
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
    }
}
