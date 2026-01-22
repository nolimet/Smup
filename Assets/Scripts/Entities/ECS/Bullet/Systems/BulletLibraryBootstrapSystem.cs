using System;
using System.Collections.Generic;
using Entities.ECS.Bullet.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using BoxCollider = Unity.Physics.BoxCollider;

namespace Entities.ECS.Bullet.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class BulletLibraryBootstrapSystem : SystemBase
    {
        private static readonly Dictionary<Sprite, Mesh> SpriteMeshes = new();

        protected override void OnCreate()
        {
            RequireForUpdate<BulletLibraryTag>();
        }

        protected override void OnUpdate()
        {
            var config = Resources.Load<BulletConfig>("Configuration/BulletConfig");
            if (config == null)
            {
                Debug.LogError("BulletConfig not found in Resources.");
                Enabled = false;
                return;
            }

            var em = EntityManager;

            var libraryEntity = SystemAPI.GetSingletonEntity<BulletLibraryTag>();
            if (!em.HasBuffer<BulletPrefabContainer>(libraryEntity))
                em.AddBuffer<BulletPrefabContainer>(libraryEntity);

            var entries = new List<BulletPrefabContainer>();

            foreach (var entry in config.entries)
            {
                var prefab = em.CreateEntity();

                em.AddComponentData(prefab, new BulletData());
                em.AddComponentData(prefab, new PhysicsVelocity());
                em.AddComponentData(prefab, PhysicsMass.CreateDynamic(
                    MassProperties.CreateBox(new float3(entry.colliderSize.x, entry.colliderSize.y, 0.1f)),
                    entry.mass
                ));

                var collider = BoxCollider.Create(new BoxGeometry
                {
                    Center = float3.zero,
                    Size = new float3(entry.colliderSize.x, entry.colliderSize.y, 0.1f),
                    Orientation = quaternion.identity,
                    BevelRadius = 0.0f
                });
                em.AddComponentData(prefab, new PhysicsCollider { Value = collider });

                var spriteMesh = GetOrCreateSpriteMesh(entry.sprite);

                var desc = new RenderMeshDescription(
                    shadowCastingMode: ShadowCastingMode.Off,
                    receiveShadows: false
                );

                var renderMeshArray = new RenderMeshArray(
                    new[] { entry.material },
                    new[] { spriteMesh }
                );

                RenderMeshUtility.AddComponents(
                    prefab,
                    em,
                    desc,
                    renderMeshArray,
                    MaterialMeshInfo.FromRenderMeshArrayIndices(0, 0)
                );

                em.AddComponent<Prefab>(prefab);

                entries.Add(new BulletPrefabContainer
                {
                    Type = entry.type,
                    Prefab = prefab
                });
            }

            var buffer = em.GetBuffer<BulletPrefabContainer>(libraryEntity);
            foreach (var item in entries)
                buffer.Add(item);

            Enabled = false;
        }

        private static Mesh GetOrCreateSpriteMesh(Sprite sprite)
        {
            if (SpriteMeshes.TryGetValue(sprite, out var cached))
                return cached;

            var mesh = new Mesh
            {
                vertices = Array.ConvertAll(sprite.vertices, v => (Vector3)v),
                uv = sprite.uv,
                triangles = Array.ConvertAll(sprite.triangles, t => (int)t)
            };
            SpriteMeshes[sprite] = mesh;
            return mesh;
        }
    }
}
