using System;
using System.IO;
using System.Linq;
using Entities.Enemies.Movement;
using JetBrains.Annotations;
using Pools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Splines;

namespace Tools.Enemies
{
    public class SplineSpawner : MonoBehaviour
    {
        [SerializeField] [ReadOnly] private SplineContainer splineComponent;

        [SerializeField] [ValueDropdown(nameof(Enemies))] private string enemyId;
        [SerializeField] [Min(0)] private float speed = 1;

        private string[] Enemies => Directory.GetFiles("Assets/Resources/Enemies", "*.prefab")
            .Select(x => x.Split('/', '\\').Last().Split('.').First()).ToArray();

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
