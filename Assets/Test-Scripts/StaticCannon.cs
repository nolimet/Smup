using Cysharp.Threading.Tasks;
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
        public Vector2 inheritVelocity;

        private void Start()
        {
            Gun = new Cannon();
            enabled = false;
            UniTask.Delay(1000).ContinueWith(BulletSpawner.Init).ContinueWith(() => enabled = true).Forget();
        }

        private void Update()
        {
            Gun.TryShoot(transform.position, spawnPosition, inheritVelocity);
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
