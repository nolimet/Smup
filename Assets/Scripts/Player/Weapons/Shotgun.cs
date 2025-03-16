using System;
using Generic_Objects;
using Managers;
using ObjectPools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Weapons
{
    public class ShotGun : IBaseWeapon
    {
        private DateTime _lastShot;

        private readonly int _bulletsPerShot;

        //number of millisecond of delay per shot
        private readonly float _fireDelay;
        private readonly float _accuracy;
        private readonly float _bulletSpeed;
        private readonly float _bulletDamage;

        private readonly int _energyCost;
        public float EnergyCost => _energyCost;

        public float DelayDelta => (float)(DateTime.Now - _lastShot).TotalMilliseconds / _fireDelay;

        public ShotGun()
        {
            const int fireRate = 24;
            _bulletDamage = 0.5f * Mathf.Pow(1.3f, GameManager.Upgrades.shotgun.damage);
            _accuracy = 45 * Mathf.Pow(0.9f, GameManager.Upgrades.shotgun.accuracy);
            _bulletSpeed = 7f * Mathf.Pow(1.1f, 1);

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

            _energyCost = 30;
            _bulletsPerShot *= Mathf.RoundToInt(5 * Mathf.Pow(1.2f, GameManager.Upgrades.shotgun.fireRate));
        }

        public bool TryShoot(GameObject entiy, Vector3 weaponOffSet, Vector2 inherentVelocity)
        {
            if ((DateTime.Now - _lastShot).TotalMilliseconds >= _fireDelay)
            {
                _lastShot = DateTime.Now;
                for (var i = 0; i < _bulletsPerShot; i++)
                {
                    var bullet = BulletPool.GetBullet(BulletGeneric.Type.Bullet);
                    var angle = Random.Range(-0.5f, 0.5f) * _accuracy;

                    bullet.transform.position = entiy.transform.position + weaponOffSet;
                    bullet.Init(_bulletDamage, angle, _bulletSpeed);
                }

                return true;
            }

            return false;
        }
    }
}
