using System.Collections.Generic;
using System.Linq;
using Enemies_old;
using ObjectPools;
using UnityEngine;
using Util.Saving;
using Vector3 = UnityEngine.Vector3;

namespace Managers
{
    public class WaveController : MonoBehaviour
    {
        public delegate void WaveComplete();

        public event WaveComplete WaveCompleted;

        [SerializeField] private int enemiesLeftInWave;

        [SerializeField] private Vector2 waveStartOffset = Vector2.zero;
        public List<EnemyStats> currentEnemies;

        private Dictionary<Vector2, char>[] _patterns;

        private void Start()
        {
            EnemyPool.Instance.Removed += EnemyPoolRemoved;
            currentEnemies = new List<EnemyStats>();

            var wave = Serialization.Load<WaveClass>("Wave1", Serialization.FileTypes.Wave, false);
            Debug.Log(wave);

            var waveDict = wave.Convert();

            _patterns = new Dictionary<Vector2, char>[(int)waveDict.Max(x => x.Key.z) + 1];

            foreach (var key in waveDict.Keys)
            {
                if (_patterns[(int)key.z] == null)
                    _patterns[(int)key.z] = new Dictionary<Vector2, char>();
                _patterns[(int)key.z].Add(key, waveDict[key]);
            }

            if (_patterns.Length == 0)
            {
                enabled = false;
                return;
            }

            CreateWave();
        }

        private void OnDestroy()
        {
            EnemyPool.Instance.Removed -= EnemyPoolRemoved;
        }

        private void EnemyPoolRemoved(EnemyStats enemy)
        {
            enemiesLeftInWave--;
            currentEnemies.Remove(enemy);
        }

        private void Update()
        {
            if (enemiesLeftInWave <= 0)
            {
                WaveCompleted?.Invoke();

                CreateWave();
            }
        }

        public void CreateWave()
        {
            enemiesLeftInWave = 0;
            if (_patterns == null)
            {
                Debug.LogError("NO PATTERNS LOADED!");
                enabled = false;
                return;
            }

            var w = Random.Range(0, _patterns.Length);

            foreach (var v in _patterns[w].Keys) AddEnemy(v + waveStartOffset, (EnemyStats.Type)_patterns[w][v]);
        }

        public void AddEnemy(Vector3 pos, EnemyStats.Type type)
        {
            enemiesLeftInWave++;
            var e = EnemyPool.GetEnemy(type);

            e.transform.position = pos;
            e.transform.rotation = Quaternion.identity;

            currentEnemies.Add(e);
        }
    }
}
