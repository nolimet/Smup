using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

namespace Pools
{
    public interface IPool<TPoolElement> : IDisposable where TPoolElement : Component, IPoolElement
    {
        int ActiveItems { get; }
        TPoolElement GetObject(string poolId);
        void ReleaseObject(TPoolElement obj);
    }

    public abstract class BasePool<TPoolElement, TSelf> : MonoBehaviour, IPool<TPoolElement> where TPoolElement : Component, IPoolElement where TSelf : BasePool<TPoolElement, TSelf>
    {
        public static IPool<TPoolElement> Instance;

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = (TSelf)this;
            else
                Destroy(this);

            _rootObject = new GameObject(typeof(TSelf).Name);
            _rootObject.transform.SetParent(transform);
        }

        protected abstract string BasePath { get; }

        [ShowInInspector]
        private readonly Dictionary<string, PoolContainer> _pools = new();

        [ShowInInspector]
        public int ActiveItems
        {
            get
            {
                var count = 0;
                foreach (var container in _pools.Values) count += container.ActiveCount;
                return count;
            }
        }

        private GameObject _rootObject;

        public TPoolElement GetObject(string poolId)
        {
            if (!_pools.TryGetValue(poolId, out var pool)) _pools.Add(poolId, pool = new PoolContainer(poolId, BasePath, _rootObject.transform));
            return pool.Get();
        }

        public void ReleaseObject(TPoolElement obj)
        {
            _pools[obj.PoolId].Release(obj);
        }

        public void Dispose()
        {
            foreach (var poolContainer in _pools.Values) poolContainer.Dispose();
            _pools.Clear();
        }

        protected class PoolContainer : IDisposable
        {
            private readonly GameObject _prefab;
            private readonly ObjectPool<TPoolElement> _pool;
            private readonly string _basePath;
            private readonly Transform _root;

            public int ActiveCount => _pool.CountActive;

            public PoolContainer(string type, string basePath, Transform root)
            {
                _root = root;
                _prefab = Resources.Load<GameObject>($"{basePath}/{type}");
                _pool = new ObjectPool<TPoolElement>(CreateElement, OnGet, OnRelease, OnDestroy, maxSize: 20);
            }

            public TPoolElement Get() => _pool.Get();

            public void Release(TPoolElement obj)
            {
                _pool.Release(obj);
            }

            private static void OnDestroy(TPoolElement obj)
            {
                Destroy(obj.gameObject);
            }

            private static void OnGet(TPoolElement obj)
            {
                obj.gameObject.SetActive(true);
                obj.OnSpawn();
            }

            private static void OnRelease(TPoolElement obj)
            {
                obj.gameObject.SetActive(false);
                obj.OnDespawn();
            }

            private TPoolElement CreateElement()
            {
                var newInstance = Instantiate(_prefab, _root, true);
                return newInstance.GetComponent<TPoolElement>();
            }

            public void Dispose()
            {
                _pool?.Dispose();
            }
        }
    }
}
