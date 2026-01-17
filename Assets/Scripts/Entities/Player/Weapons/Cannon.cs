using System;
using Entities.ECS.Bullet;
using Entities.Generic;
using Managers;
using Sirenix.OdinInspector;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Player.Weapons
{
    public class Cannon : IBaseWeapon
    {
        private DateTime _lastShot;

        [ShowInInspector] private readonly int _bulletsPerShot;

        [ShowInInspector] private readonly float _fireDelay; //in miliseconds
        [ShowInInspector] private readonly float _accuracy;
        [ShowInInspector] private readonly float _bulletSpeed;
        [ShowInInspector] private readonly float _bulletDamage;

        [ShowInInspector] private readonly int _energyCost;
        [ShowInInspector] public float EnergyCost => _energyCost;
        [ShowInInspector] public float DelayDelta => (float)(DateTime.Now - _lastShot).TotalMilliseconds / _fireDelay;

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

        public bool TryShoot(Vector3 shooterPosition, Vector3 weaponOffSet, Vector2 inherentVelocity)
        {
            if ((DateTime.Now - _lastShot).TotalMilliseconds >= _fireDelay)
            {
                _lastShot = DateTime.Now;
                var totalPosition = shooterPosition + weaponOffSet;
                var position = new float3(totalPosition.x, totalPosition.y, totalPosition.z);
                var inheritVelocity = new float3(inherentVelocity.x, inherentVelocity.y, 0);
                for (var i = 0; i < _bulletsPerShot; i++)
                {
                    var angle = Random.Range(-0.5f, 0.5f) * _accuracy;
                    BulletSpawner.Shoot(BulletGeneric.BulletType.Bullet, position, inheritVelocity, angle, _bulletSpeed, _bulletDamage);
                }

                return true;
            }

            return false;
        }
    }
}
