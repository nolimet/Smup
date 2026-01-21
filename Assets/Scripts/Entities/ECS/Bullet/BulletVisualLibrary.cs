using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Entities.ECS.Bullet
{
    public struct BulletLibaryTag : IComponentData { }

    public struct BulletVisualElement : IBufferElementData
    {
        public BulletType Type;
        public Entity Prefab;
    }

    public class BulletVisualLibrary : MonoBehaviour
    {
        [Serializable]
        public struct BulletVisualMapping
        {
            public BulletType type;
            public GameObject prefab; // Each prefab has its own Sprite/Material
        }

        public List<BulletVisualMapping> visuals;

        private class Baker : Baker<BulletVisualLibrary>
        {
            public override void Bake(BulletVisualLibrary authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var buffer = AddBuffer<BulletVisualElement>(entity);
                AddComponent(entity, new BulletLibaryTag());

                foreach (var map in authoring.visuals)
                    buffer.Add(new BulletVisualElement
                    {
                        Type = map.type,
                        Prefab = GetEntity(map.prefab, TransformUsageFlags.Dynamic | TransformUsageFlags.Renderable)
                    });
            }
        }
    }
}
