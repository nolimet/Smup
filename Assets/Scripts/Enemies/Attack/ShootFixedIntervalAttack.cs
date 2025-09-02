using Data;
using UnityEngine;

namespace Enemies.Attack
{
    public class ShootFixedIntervalAttack : IAttack
    {
        private Overrideable<float> _salvoInterval;
        private Overrideable<float> _shotInterval;
        private Overrideable<int> _salvoSize;

        private Overrideable<float> _damagePerShot;

        private int _shotsLeft;
        private float _timer;

        public void Attack(GameObject entity)
        {
            if (_shotsLeft > 0 && _timer <= 0)
            {
                Shoot();
                _shotsLeft--;

                if (_shotsLeft > 0)
                    _timer = _shotInterval;
                else
                    _timer = _salvoInterval;
            }
            else if (_shotsLeft == 0 && _timer <= 0)
            {
                _shotsLeft = _salvoSize;
            }

            _timer -= Time.deltaTime;
        }

        private void Shoot() { }
    }
}
