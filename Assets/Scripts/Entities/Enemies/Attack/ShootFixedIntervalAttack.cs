using System;
using Data;
using Entities.Generic;
using Pools;
using UnityEngine;

namespace Entities.Enemies.Attack
{
    [Serializable]
    public class ShootFixedIntervalAttack : IAttack
    {
        [SerializeField] private Overrideable<float> salvoInterval = 0.5f;
        [SerializeField] private Overrideable<float> shotInterval = 0.1f;
        [SerializeField] private Overrideable<int> salvoSize = 3;

        [SerializeField] private Overrideable<float> damagePerShot = 5;
        [SerializeField] private Overrideable<float> shotSpeed = 5;

        private int _shotsLeft;
        private float _timer;

        public void Attack(Vector2 position, Vector2 motionVector)
        {
            if (_shotsLeft > 0 && _timer <= 0)
            {
                Shoot(position, motionVector);
                _shotsLeft--;

                if (_shotsLeft > 0)
                    _timer = shotInterval;
                else
                    _timer = salvoInterval;
            }
            else if (_shotsLeft == 0 && _timer <= 0)
            {
                _shotsLeft = salvoSize;
            }

            _timer -= Time.deltaTime;
        }

        private void Shoot(Vector2 position, Vector2 motionVector)
        {
            var bullet = BulletPool.Instance.GetObject(BulletGeneric.BulletType.Bullet);
            bullet.transform.position = position;
            bullet.Init(damagePerShot, -180, motionVector.x + shotSpeed, LayerMask.NameToLayer("Player"));
            bullet.gameObject.layer = LayerMask.NameToLayer("EnemyBullets");
        }
    }
}
