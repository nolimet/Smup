using Sirenix.OdinInspector;
using Smup.Entities.Enemies;
using Smup.Pools;
using UnityEngine;

namespace Smup.Test_Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private string enemyId;
        [SerializeField] [ReadOnly] private Enemy currentTarget;

        [SerializeField] private float spawnDelay = 0.2f;
        private float _spawnTimer;

        private void Spawn()
        {
            currentTarget = EnemyPool.Instance.GetObject(enemyId);
            currentTarget.moveSpeed.SetOverride(0);
            currentTarget.transform.position = transform.position;
            var rig2d = currentTarget.GetComponent<Rigidbody2D>();
            rig2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        private void Update()
        {
            if (!currentTarget || currentTarget.Health <= 0)
            {
                _spawnTimer -= Time.deltaTime;
                if (_spawnTimer <= 0)
                {
                    Spawn();
                    _spawnTimer = spawnDelay;
                }
            }
        }
    }
}
