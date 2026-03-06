using Entities.ECS.Enemies.Systems;
using Unity.Entities;
using UnityEngine;

namespace Entities.Enemies
{
	/// <summary>
	/// Add this component to MonoBehaviour Enemy GameObjects to make them
	/// interact with the ECS collision system.
	/// </summary>
	[RequireComponent(typeof(Enemy))]
	public class EnemyECSBridge : MonoBehaviour
	{
		[SerializeField] private float colliderRadius = 0.5f;
		
		private Enemy _enemy;
		private Entity _ecsEntity;
		private HybridEnemyBridgeSystem _bridgeSystem;
		private bool _isInitialized;

		private void Awake()
		{
			_enemy = GetComponent<Enemy>();
		}

		private void Start()
		{
			InitializeECSEntity();
		}

		private void InitializeECSEntity()
		{
			if (_isInitialized) return;

			var world = World.DefaultGameObjectInjectionWorld;
			if (world == null) return;

			_bridgeSystem = world.GetOrCreateSystemManaged<HybridEnemyBridgeSystem>();
			_ecsEntity = _bridgeSystem.CreateEnemyEntity(_enemy, colliderRadius);
			_isInitialized = true;
		}

		private void Update()
		{
			if (!_isInitialized) return;

			// Sync position every frame
			_bridgeSystem.SyncEnemyPosition(_ecsEntity, transform.position);
			_bridgeSystem.SyncEnemyHealth(_ecsEntity, _enemy.Health);
		}

		private void OnDestroy()
		{
			if (_isInitialized && _bridgeSystem != null)
			{
				_bridgeSystem.RemoveEnemyEntity(_ecsEntity);
			}
		}

		private void OnEnable()
		{
			if (!_isInitialized && _enemy != null)
			{
				InitializeECSEntity();
			}
		}

		private void OnDisable()
		{
			if (_isInitialized && _bridgeSystem != null)
			{
				_bridgeSystem.RemoveEnemyEntity(_ecsEntity);
				_isInitialized = false;
			}
		}
	}
}

