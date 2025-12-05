using System.Collections.Generic;
using System.IO;
using System.Linq;
using Enums;
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

		[SerializeField] private Vector2 waveStartOffset = Vector2.zero;

		private Dictionary<Vector2, char>[] _patterns;

		private void Start()
		{
			if (!Serialization.TryLoadWave("Wave1", out var wave))
			{
				throw new FileLoadException("Could not load wave file!");
			}
			Debug.Log(wave);

			var waveDict = wave.Convert();

			_patterns = new Dictionary<Vector2, char>[(int) waveDict.Max(x => x.Key.z) + 1];

			foreach (var key in waveDict.Keys)
			{
				if (_patterns[(int) key.z] == null)
				{
					_patterns[(int) key.z] = new Dictionary<Vector2, char>();
				}
				_patterns[(int) key.z].Add(key, waveDict[key]);
			}

			if (_patterns.Length == 0)
			{
				enabled = false;
				return;
			}

			CreateWave();
		}

		private void Update()
		{
			if (EnemyPool.Instance.ActiveItems <= 0)
			{
				WaveCompleted?.Invoke();

				CreateWave();
			}
		}

		public void CreateWave()
		{
			if (_patterns == null)
			{
				Debug.LogError("NO PATTERNS LOADED!");
				enabled = false;
				return;
			}

			var w = Random.Range(0, _patterns.Length);

			foreach (var v in _patterns[w].Keys) AddEnemy(v + waveStartOffset, ((EnemyType) _patterns[w][v]).ToString());
		}

		public void AddEnemy(Vector3 pos, string type)
		{
			var e = EnemyPool.Instance.GetObject(type);

			e.transform.position = pos;
			e.transform.rotation = Quaternion.identity;
		}
	}
}
