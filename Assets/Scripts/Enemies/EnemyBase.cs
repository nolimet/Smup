using UnityEngine;
using Enemy.Interfaces.Attack;
using Enemy.Interfaces.Move;
using System.Collections;

namespace Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyBase : MonoBehaviour
    {
        IAttack attackPattern;
        IMovement movementPattern;

        void Start()
        {
            attackPattern = new ShootFixedIntervalAttack();
            movementPattern = new Enemy.Interfaces.Move.LinearMovement();
            attackPattern.Weapon = new WeaponInterfaces.MiniGun();
           
        }

        void Update()
        {
            attackPattern.Attack(gameObject);
            movementPattern.Move(gameObject, 4);
        }
    }
}