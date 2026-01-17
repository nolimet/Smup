using Entities.ECS.Bullet;
using Entities.Player.Weapons;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Test_Scripts
{
    public class StaticCannon : MonoBehaviour
    {
        [ShowInInspector] public IBaseWeapon Gun;
        public Vector3 spawnPosition = new(0.5f, 0);
        public Vector2 inheritVelocity = new();

        public float shootInterval = 0.2f;
        private float _timer = 0;

        private void Start()
        {
            Gun = new Cannon();
            _timer = shootInterval;
            BulletSpawner.Init();
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Gun.TryShoot(transform.position, spawnPosition, inheritVelocity);
                _timer = shootInterval;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(transform.position + spawnPosition, 0.1f);
        }

        private void OnDestroy()
        {
            BulletSpawner.DeInit();
        }
    }
}
