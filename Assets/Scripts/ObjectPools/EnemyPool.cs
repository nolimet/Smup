using System.Collections.Generic;
using System.Linq;
using Enemies_old;
using UnityEngine;

namespace ObjectPools
{
    public class EnemyPool : MonoBehaviour
    {
        public static EnemyPool Instance;

        public delegate void EnemyActivty(EnemyStats enemy);

        public event EnemyActivty Removed;

        private List<EnemyStats> _activePool, _inActivePool;

        private bool _autoCollectEnemiesOnStart = true;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);

            _activePool = new List<EnemyStats>();
            _inActivePool = new List<EnemyStats>();
        }

        private void Start()
        {
            if (_autoCollectEnemiesOnStart)
            {
                var pl = FindObjectsOfType<EnemyStats>();
                foreach (var p in pl)
                {
                    if (p.gameObject.activeSelf)
                        _activePool.Add(p);
                    else
                        _inActivePool.Add(p);

                    p.transform.SetParent(transform);
                }
            }
        }

        private void Update()
        {
            if (Instance == null)
                Instance = this;
        }

        public static void RemoveEnemy(EnemyStats e)
        {
            if (Instance)
            {
                if (Instance._activePool.Contains(e))
                    Instance._activePool.Remove(e);
                if (!Instance._inActivePool.Contains(e))
                    Instance._inActivePool.Add(e);
                e.gameObject.SetActive(false);

                Instance.Removed(e);
            }
        }

        public static EnemyStats GetEnemy(EnemyStats.Type type)
        {
            if (Instance)
            {
                EnemyStats e;
                if (Instance._inActivePool.Any(i => i.type == type))
                {
                    e = Instance._inActivePool.First(i => i.type == type);
                    Instance._inActivePool.Remove(e);
                    Instance._activePool.Add(e);
                    e.gameObject.SetActive(true);
                }
                else
                {
                    var g = Instantiate(Resources.Load($"Enemies/{type}"), Vector3.zero, Quaternion.identity) as GameObject;
                    e = g.GetComponent<EnemyStats>();

                    Instance._activePool.Add(e);
                    e.transform.SetParent(Instance.transform, false);
                }

                e.gameObject.SendMessage("StartBehaviours");
                return e;
            }

            return null;
        }
    }
}
