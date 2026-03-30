using Sirenix.OdinInspector;
using Smup.Entities.Enemies;
using UnityEngine;

namespace Smup.Tools
{
    public class EnemyGroup : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
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
