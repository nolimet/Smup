using Enemies;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace LevelTools
{
    public class EnemyGroup : MonoBehaviour
    {
        [FormerlySerializedAs("_moveSpeed")] [SerializeField] private float moveSpeed;
        [ReadOnly] private Enemy[] _enemies;

        private void Start()
        {
            foreach (var enemy in _enemies)
            {
                enemy.OnSpawn();
                enemy.moveSpeed.SetOverride(moveSpeed);
            }
        }

        private void OnValidate()
        {
            _enemies = GetComponentsInChildren<Enemy>();
        }
    }
}
