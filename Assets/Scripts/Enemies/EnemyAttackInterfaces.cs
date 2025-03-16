using UnityEngine;

namespace Enemies
{
    public interface IAttack
    {
        void Attack(GameObject enity);
    }

    public class ShootFixedIntervalAttack : IAttack
    {
        private float _counter;
        private int _fireRepeator;

        public void Attack(GameObject enity)
        {
            if (_fireRepeator <= 0 && _counter >= 5)
            {
                _counter = 0f;
                _fireRepeator = 5;
            }
            else if (_fireRepeator > 0 && _counter >= 0.3f)
            {
                _counter = 0f;
                _fireRepeator--;
            }

            _counter += Time.deltaTime;
        }
    }
}
