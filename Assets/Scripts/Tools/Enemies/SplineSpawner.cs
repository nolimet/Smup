using System;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Smup.Entities.Enemies.Movement;
using Smup.Helpers;
using Smup.Pools;
using UnityEngine;
using UnityEngine.Splines;

namespace Smup.Tools.Enemies
{
    public class SplineSpawner : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private SplineContainer splineComponent;

        [SerializeField] [ValueDropdown(nameof(Enemies))] private string enemyId;
        [SerializeField] [Min(0)] private float speed = 1;

        private string[] Enemies => EnemyTypeHelper.GetEnemyTypes();

        private void Awake()
        {
            splineComponent = GetComponentInParent<SplineContainer>();

            if (splineComponent == null) Debug.LogError("SplineContainer not found on parent of " + gameObject.name);
        }

        [PublicAPI]
        public void Spawn(int track)
        {
            if (!splineComponent)
                throw new NullReferenceException();

            var enemy = EnemyPool.Instance.GetObject(enemyId);

            if (enemy.MovementPattern is SplineFollow splineFollow)
                splineFollow.SetSpline(splineComponent.Splines[track]);
            else Debug.LogWarning($"Enemy does not have SplineFollow movement pattern {enemyId}", this);
            enemy.moveSpeed.SetOverride(speed);
        }
    }
}
