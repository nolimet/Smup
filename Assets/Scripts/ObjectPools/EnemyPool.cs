using System.Collections.Generic;
using Enemies;
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPools
{
    public static class EnemyPool
    {
        private static readonly Dictionary<string, PoolContainer> _pools = new();

        public static int ActiveEnemies
        {
            get
            {
                var count = 0;
                foreach (var container in _pools.Values) count += container.ActiveCount;
                return count;
            }
        }

        public static EnemyBase GetEnemy(string type)
        {
            if (!_pools.TryGetValue(type, out var pool)) _pools.Add(type, pool = new PoolContainer(type));
            return pool.Get();
        }

        public static void ReleaseEnemy(EnemyBase obj)
        {
            _pools[obj.TypeName].Release(obj);
        }

        private class PoolContainer
        {
            private readonly GameObject _prefab;
            private readonly ObjectPool<EnemyBase> _pool;
            private readonly string type;

            public int ActiveCount => _pool.CountActive;

            public PoolContainer(string type)
            {
                _pool = new ObjectPool<EnemyBase>(CreateElement, GetPrefab, OnRelease, OnDestroy, maxSize: 20);
            }

            public EnemyBase Get()
            {
                return _pool.Get();
            }

            public void Release(EnemyBase obj)
            {
                _pool.Release(obj);
            }

            private void OnDestroy(EnemyBase obj)
            {
                Object.Destroy(obj.gameObject);
            }

            private void GetPrefab(EnemyBase obj)
            {
                obj.gameObject.SetActive(true);
                obj.OnSpawn();
            }

            private void OnRelease(EnemyBase obj)
            {
                obj.gameObject.SetActive(false);
            }

            private EnemyBase CreateElement()
            {
                var newInstance = Object.Instantiate(_prefab);
                return newInstance.GetComponent<EnemyBase>();
            }
        }
    }
}
