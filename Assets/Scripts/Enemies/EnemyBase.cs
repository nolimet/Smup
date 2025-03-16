using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBase : MonoBehaviour
    {
        private IAttack _attackPattern;
        private IMovement _movementPattern;

        private void Start()
        {
            _attackPattern = new ShootFixedIntervalAttack();
            _movementPattern = new LinearMovement();
            //attackPattern.Weapon = new WeaponInterfaces.MiniGun();
        }

        private void Update()
        {
            _attackPattern.Attack(gameObject);
            _movementPattern.Move(gameObject, speed: 1f);
        }
    }
}
