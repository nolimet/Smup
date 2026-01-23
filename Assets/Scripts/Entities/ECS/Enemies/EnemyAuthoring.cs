using Unity.Entities;
using UnityEngine;

namespace Entities.ECS.Enemies
{
    public struct EnemyData : IComponentData
    {
        public bool hasBeenOnScreen;

        public double health;
        public double scrapValue;
        public double scrapCloudSize;
        public double contactDamage;
    }

    public class EnemyAuthoring : MonoBehaviour
    {
        [SerializeField] [Min(1)] private double health;
        [SerializeField] [Min(0)] private double scrapValue;
        [SerializeField] [Min(0)] private double scrapCloudSize;
        [SerializeField] [Min(0)] private double contactDamage;

        private class Baker : Baker<EnemyAuthoring>
        {
            public override void Bake(EnemyAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new EnemyData
                {
                    health = authoring.health,
                    scrapValue = authoring.scrapValue,
                    scrapCloudSize = authoring.scrapCloudSize,
                    contactDamage = authoring.contactDamage
                });
            }
        }
    }
}
