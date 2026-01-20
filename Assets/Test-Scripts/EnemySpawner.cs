using Entities.Enemies;
using Pools;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Test_Scripts
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
            var rig = currentTarget.GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.FreezeAll;
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
