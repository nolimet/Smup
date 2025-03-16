using UnityEngine;

namespace Enemies
{
    public interface IAttack
    {
        void Attack(GameObject enity);
    }

    public class ShootFixedIntervalAttack : IAttack
    {
        //Shot a number of shots every so many seconds
        private readonly float _salvoInterval;
        private readonly int _salvoSize;
        private readonly float _shotInterval;

        private float _salvoTimer;
        private float _shotTimer;
        private float _salvoCounter;

        public void Attack(GameObject enity)
        {
            if (_salvoCounter == 0 && _salvoCounter > _salvoInterval)
            {
                _salvoCounter = _salvoSize;
                _shotTimer = 0;
            }
            else if (_salvoCounter > 0 && _shotInterval <= 0)
            {
                _salvoTimer = _shotInterval;
                _salvoCounter--;
                //TODO Implement shoot
            }

            _shotTimer -= Time.deltaTime;
            _salvoTimer -= Time.deltaTime;
        }
    }
}
