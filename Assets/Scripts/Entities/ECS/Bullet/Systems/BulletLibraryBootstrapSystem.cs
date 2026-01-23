using System;
using System.Collections.Generic;
using Entities.ECS.Bullet.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Rendering;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.Rendering;
using BoxCollider = Unity.Physics.BoxCollider;
using InitializationSystemGroup = Unity.Entities.InitializationSystemGroup;
using Material = UnityEngine.Material;
using Prefab = Unity.Entities.Prefab;
using SceneSection = Unity.Entities.SceneSection;
using SceneTag = Unity.Entities.SceneTag;
using SystemAPI = Unity.Entities.SystemAPI;
using SystemBase = Unity.Entities.SystemBase;

namespace Entities.ECS.Bullet.Systems
{
    [UpdateInGroup(typeof(InitializationSystemGroup))]
    public partial class BulletLibraryBootstrapSystem : SystemBase
    {
        private static readonly Dictionary<Sprite, Mesh> SpriteMeshes = new();
        private static readonly Dictionary<(Mesh, Material), RenderMeshArray> RenderMeshArrays = new();

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

            var hasSceneSection = em.HasComponent<SceneSection>(libraryEntity);
            var hasSceneTag = em.HasComponent<SceneTag>(libraryEntity);

            var sceneSection = hasSceneSection
                ? em.GetSharedComponentManaged<SceneSection>(libraryEntity)
                : default;

            var entries = new List<BulletPrefabContainer>();
            foreach (var entry in config.entries)
            {
                var prefab = em.CreateEntity();

                if (hasSceneSection)
                    em.AddSharedComponentManaged(prefab, sceneSection);
                if (hasSceneTag)
                    em.AddComponent<SceneTag>(prefab);

                em.AddSharedComponentManaged(prefab, new BulletTypeShared { Value = entry.type });
                em.AddComponentData(prefab, new BulletData());
                em.AddComponentData(prefab, new PhysicsVelocity());
                em.AddComponentData(prefab, PhysicsMass.CreateDynamic(
                    MassProperties.CreateBox(new float3(entry.colliderSize.x, entry.colliderSize.y, 0.1f)),
                    entry.mass
                ));
                em.AddComponentData(prefab, LocalTransform.FromPositionRotationScale(
                    float3.zero,
                    quaternion.identity,
                    1f
                ));
                em.AddSharedComponent(prefab, new PhysicsWorldIndex { Value = 0 });

                var collider = BoxCollider.Create(new BoxGeometry
                {
                    Center = float3.zero,
                    Size = new float3(entry.colliderSize.x, entry.colliderSize.y, 0.1f),
                    Orientation = quaternion.identity,
                    BevelRadius = 0.0f
                }, new CollisionFilter
                {
                    BelongsTo = entry.belongsToMask,
                    CollidesWith = entry.CollidesWithMask,
                    GroupIndex = 0
                });
                em.AddComponentData(prefab, new PhysicsCollider { Value = collider });

                var spriteMesh = GetOrCreateSpriteMesh(entry.sprite);

                if (!RenderMeshArrays.TryGetValue((spriteMesh, entry.material), out var renderMeshArray))
                {
                    renderMeshArray = new RenderMeshArray(
                        new[] { entry.material },
                        new[] { spriteMesh }
                    );
                    RenderMeshArrays.Add((spriteMesh, entry.material), renderMeshArray);
                }

                var desc = new RenderMeshDescription(
                    shadowCastingMode: ShadowCastingMode.Off,
                    receiveShadows: false
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
