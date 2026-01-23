using System;
using Managers;
using UnityEngine;

namespace Entities.Player.Weapons
{
    public class MiniGun : IBaseWeapon
    {
        private DateTime _lastShot;

        private readonly int _bulletsPerShot;

        //number of millisecond of delay per shot
        private readonly float _fireDelay;
        private readonly float _accuracy;
        private readonly float _bulletSpeed;
        private readonly float _bulletDamage;

        public float EnergyCost { get; }

        public float DelayDelta => (float)(DateTime.Now - _lastShot).TotalMilliseconds / _fireDelay;

        public MiniGun()
        {
            var fireRate = Mathf.FloorToInt(3 * Mathf.Pow(1.2f, SaveDataManager.Upgrades.Minigun.FireRate) * 600);
            _bulletDamage = 1f * Mathf.Pow(1.3f, SaveDataManager.Upgrades.Minigun.Damage);
            _accuracy = 13 * Mathf.Pow(0.9f, SaveDataManager.Upgrades.Minigun.Accuracy);
            _bulletSpeed = 9f * Mathf.Pow(1.1f, 1);

            _fireDelay = 60000f / fireRate;

            if (_fireDelay < 1000 / 60f)
            {
                var temp = _fireDelay;
                _bulletsPerShot = 1;
                while (_fireDelay < 1000 / 60f)
                {
                    _fireDelay += temp;
                    _bulletsPerShot++;
                }
            }
            else
            {
                _bulletsPerShot = 1;
            }

            EnergyCost = 0.7f * _bulletsPerShot;
        }

        public bool TryShoot(Vector3 shooterPosition, Vector3 weaponOffSet, Vector2 inherentVelocity) =>
            /*if ((DateTime.Now - _lastShot).TotalMilliseconds >= _fireDelay)
            {
                _lastShot = DateTime.Now;
                for (var i = 0; i < _bulletsPerShot; i++)
                {
                    var bullet = BulletPool.Instance.GetObject(nameof(BulletGeneric.BulletType.Bullet));
                    var angle = Random.Range(-0.5f, 0.5f) * _accuracy;

                    bullet.transform.position = entiy.transform.position + weaponOffSet;
                    bullet.Init(_bulletDamage, angle, _bulletSpeed, LayerMask.NameToLayer("Enemy"));
                    bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
                }

                return true;
            }*/
            false;
    }
}
