using System;
using Entities.Generic;
using Managers;
using Pools;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Player.Weapons
{
    public class Cannon : IBaseWeapon
    {
        private DateTime _lastShot;

        private readonly int _bulletsPerShot;

        private readonly float _fireDelay; //in miliseconds
        private readonly float _accuracy;
        private readonly float _bulletSpeed;
        private readonly float _bulletDamage;

        private readonly int _energyCost;
        public float EnergyCost => _energyCost;
        public float DelayDelta => (float)(DateTime.Now - _lastShot).TotalMilliseconds / _fireDelay;

        public Cannon()
        {
            var fireRate = Mathf.RoundToInt(75 * Mathf.Pow(1.3f, SaveDataManager.Upgrades.Cannon.FireRate));

            _bulletDamage = 7f * Mathf.Pow(1.3f, SaveDataManager.Upgrades.Cannon.Damage);
            _accuracy = 4 * Mathf.Pow(0.9f, SaveDataManager.Upgrades.Cannon.Accuracy);
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

            _energyCost = 20;
        }

        public bool TryShoot(GameObject entiy, Vector3 weaponOffSet, Vector2 inherentVelocity)
        {
            if ((DateTime.Now - _lastShot).TotalMilliseconds >= _fireDelay)
            {
                _lastShot = DateTime.Now;
                float angle;
                for (var i = 0; i < _bulletsPerShot; i++)
                {
                    var bullet = BulletPool.Instance.GetObject(nameof(BulletGeneric.BulletType.Bullet));

                    angle = Random.Range(-0.5f, 0.5f) * _accuracy;

                    bullet.transform.position = entiy.transform.position + weaponOffSet;
                    bullet.Init(_bulletDamage, angle, _bulletSpeed, LayerMask.NameToLayer("Enemy"));
                    bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
                }

                return true;
            }

            return false;
        }
    }
}
