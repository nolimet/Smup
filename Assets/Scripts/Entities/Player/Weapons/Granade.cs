using System;
using Managers;
using UnityEngine;

namespace Entities.Player.Weapons
{
    public class Granade : IBaseWeapon
    {
        private DateTime _lastShot;

        private readonly int _bulletsPerShot;

        //number of millisecond of delay per shot
        private readonly float _fireDelay;
        private readonly float _accuracy;
        private readonly float _bulletSpeed;
        private readonly float _bulletDamage;
        private readonly int _fragments;
        private readonly int _energyCost;
        public float EnergyCost => _energyCost;

        public float DelayDelta => (float)(DateTime.Now - _lastShot).TotalMilliseconds / _fireDelay;

        public Granade()
        {
            var fireRate = Mathf.RoundToInt(10 * Mathf.Pow(1.3f, SaveDataManager.Upgrades.Grenade.FireRate));

            _bulletDamage = 7f * Mathf.Pow(1.3f, SaveDataManager.Upgrades.Grenade.Damage);
            _accuracy = 4 * Mathf.Pow(0.9f, SaveDataManager.Upgrades.Grenade.Accuracy);
            _bulletSpeed = 9f * Mathf.Pow(1.1f, 1);

            _fragments = SaveDataManager.Upgrades.Grenade.Fragments + 20;

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

            _energyCost = 50;

            // bulletsPerShot *= Mathf.RoundToInt(5*Mathf.Pow(1.2f, GameManager.upgrades.ShotGunBulletsPerShot));
        }

        public bool TryShoot(Vector3 shooterPosition, Vector3 weaponOffSet, Vector2 inherentVelocity) =>
            /*if ((DateTime.Now - _lastShot).TotalMilliseconds >= _fireDelay)
            {
                _lastShot = DateTime.Now;
                float angle;
                for (var i = 0; i < _bulletsPerShot; i++)
                {
                    var bullet = BulletPool.Instance.GetObject(nameof(BulletGeneric.BulletType.Bullet));

                    angle = Random.Range(-0.5f, 0.5f) * _accuracy;

                    bullet.transform.position = entiy.transform.position + weaponOffSet;
                    bullet.Init(_bulletDamage, angle, _bulletSpeed, LayerMask.NameToLayer("Enemy"), _fragments, 6f, false);
                    bullet.gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
                }

                return true;
            }*/
            false;
    }
}
