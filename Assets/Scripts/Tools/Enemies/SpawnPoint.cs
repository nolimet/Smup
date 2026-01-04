using Helpers;
using Pools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Tools.Enemies
{
    public class SpawnPoint : MonoBehaviour
    {
        [SerializeField] [ValueDropdown(nameof(Enemies))] private string enemyId;
        [SerializeField] [Min(0)] private float speed = 1;

        private string[] Enemies => EnemyTypeHelper.GetEnemyTypes();

        public void Spawn()
        {
            var enemy = EnemyPool.Instance.GetObject(enemyId);
            enemy.transform.position = transform.position;
            enemy.moveSpeed.SetOverride(speed);
        }
    }
}
