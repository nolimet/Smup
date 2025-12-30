using System;
using Pickups;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace Pools
{
    public class ScrapPickupPool : MonoBehaviour
    {
        public static ScrapPickupPool Instance;

#if UNITY_EDITOR
        [ShowInInspector]
        private int Count => _pool?.CountActive ?? -1;
#endif

        private ObjectPool<ScrapPickup> _pool;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);

            _pool = new ObjectPool<ScrapPickup>
            (
                CreateNewScrapPickup,
                actionOnGet: pickup => pickup.gameObject.SetActive(true),
                actionOnRelease: pickup => pickup.gameObject.SetActive(false),
                maxSize: 80,
                defaultCapacity: 20
            );
        }

        public static void RemovePickup(ScrapPickup pickup)
        {
            Instance._pool.Release(pickup);
        }

        private static ScrapPickup CreateNewScrapPickup()
        {
            var gameObject = Instantiate(Resources.Load("Pickups/Scrap"), Instance.transform, false) as GameObject;
            if (!gameObject)
                throw new NullReferenceException("No scrap found");

            return gameObject.GetComponent<ScrapPickup>();
        }

        public static ScrapPickup GetPickup() => Instance._pool.Get();

        /// <summary>
        /// Create a collection of scrap
        /// </summary>
        /// <param name="pos">Where the cloud should be created on screen</param>
        /// <param name="size">How large are the bounds of the cloud</param>
        /// <param name="count">How many bits of scrap are there in the cloud</param>
        /// <param name="value">What is the combined value of the cloud</param>
        public static void CreateScrapCloud(Vector2 pos, Vector2 size, int count, float value)
        {
            for (var i = 0; i < count; i++)
            {
                var pickup = GetPickup();
                var position = new Vector2(Random.Range(size.x / -2f, size.x / 2f), Random.Range(size.y / -2f, size.y / 2f));
                pickup.transform.position = pos + position;
                pickup.Init(120, value / count);
            }
        }
    }
}
