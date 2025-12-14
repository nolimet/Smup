using System.IO;
using System.Linq;
using Enemies.Movement;
using ObjectPools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

namespace LevelTools
{
    [RequireComponent(typeof(SplineContainer))]
    public class SplineSpawner : MonoBehaviour
    {
        private SplineContainer _splineComponent;
        [SerializeField] [ValueDropdown(nameof(Enemies))] private string _enemyId;
        [FormerlySerializedAs("_speed")] [SerializeField] private float speed;

        private string[] Enemies => Directory.GetFiles("Assets/Resources/Enemies", "*.prefab")
            .Select(x => x.Split('/', '\\').Last().Split('.').First()).ToArray();

        private void Awake()
        {
            _splineComponent = GetComponent<SplineContainer>();
        }

        //TODO use a triggering system instead of this
        private void Start()
        {
            var enemy = EnemyPool.Instance.GetObject(_enemyId);

            if (enemy.MovementPattern is SplineFollow splineFollow)
                splineFollow.SetSpline(_splineComponent.Spline);
            enemy.moveSpeed = speed;
        }
    }
}
