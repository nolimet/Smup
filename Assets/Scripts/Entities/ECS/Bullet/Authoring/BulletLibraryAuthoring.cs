using Entities.ECS.Bullet.Components;
using Unity.Entities;
using UnityEngine;

namespace Entities.ECS.Bullet.Authoring
{
    public class BulletLibraryAuthoring : MonoBehaviour
    {
        private class Baker : Baker<BulletLibraryAuthoring>
        {
            public override void Bake(BulletLibraryAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new BulletLibraryTag());
            }
        }
    }
}
