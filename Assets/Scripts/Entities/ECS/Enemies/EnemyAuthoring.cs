using Entities.ECS.Enemies.Components;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
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
		[Header("Enemy Stats")]
		[SerializeField] [Min(1)] private double health;
		[SerializeField] [Min(0)] private double scrapValue;
		[SerializeField] [Min(0)] private double scrapCloudSize;
		[SerializeField] [Min(0)] private double contactDamage;

		[Header("Physics Configuration")]
		[SerializeField] private float colliderRadius = 0.5f;
		[SerializeField] private float mass = 1f;
		[SerializeField] private bool isKinematic = true;

		private class Baker : Baker<EnemyAuthoring>
		{
			public override void Bake(EnemyAuthoring authoring)
			{
				var entity = GetEntity(TransformUsageFlags.Dynamic);
				
				// Add enemy identification tag
				AddComponent(entity, new EnemyTag());
				
				// Add enemy data
				AddComponent(entity, new EnemyData
				{
					health = authoring.health,
					scrapValue = authoring.scrapValue,
					scrapCloudSize = authoring.scrapCloudSize,
					contactDamage = authoring.contactDamage
				});

				// Add physics components for ECS collision detection
				AddComponent(entity, new PhysicsVelocity());

				// Create sphere collider for the enemy
				var collider = Unity.Physics.SphereCollider.Create(
					new SphereGeometry
					{
						Center = float3.zero,
						Radius = authoring.colliderRadius
					},
					new CollisionFilter
					{
						BelongsTo = 1u << 1,      // Enemy layer (bit 1)
						CollidesWith = 1u << 0,   // Bullet layer (bit 0)
						GroupIndex = 0
					}
				);

				AddComponent(entity, new PhysicsCollider { Value = collider });

				// Add physics mass
				var massProperties = authoring.isKinematic 
					? PhysicsMass.CreateKinematic(MassProperties.UnitSphere)
					: PhysicsMass.CreateDynamic(MassProperties.UnitSphere, authoring.mass);

				AddComponent(entity, massProperties);
			}
		}
	}
}
