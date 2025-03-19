using System;
using Cysharp.Threading.Tasks;
using ObjectPools;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class EnemyBase : MonoBehaviour
    {
        private Rigidbody2D _rigidBody2D;
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;
        
        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private int scrapValue;
        [SerializeField] private Vector2 scrapCloudSize = Vector2.one * 7;
        
        [field: SerializeField] public double Health { get; private set; }
        [field: SerializeField] public double MaxHealth { get; private set; }

        private IAttack _attackPattern;
        private IMovement _movementPattern;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            _attackPattern = new ShootFixedIntervalAttack();
            _movementPattern = new LinearMovement(gameObject);
            //attackPattern.Weapon = new WeaponInterfaces.MiniGun();
        }

        private void Update()
        {
            _attackPattern.Attack(gameObject);
            _movementPattern.Move(speed: moveSpeed);
        }

        public void DoDamage(double damage)
        {
            if (Health <= 0)
                return;
            
            Health -= damage;
            if (Health <= 0)
            {
                DestroyLoop().Forget();
            }
        }

        async UniTaskVoid DestroyLoop()
        {
            while (_spriteRenderer.color.a>0.01f)
            {
                var color = _spriteRenderer.color;
                color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / fadeDuration);
                _spriteRenderer.color = color;
                await UniTask.Yield();
            }
            
            EnemyPool.RemoveEnemy(this);

            if (scrapValue > 0)
            {
                var cloudSize=Random.Range(4,20);
                ScrapPickupPool.CreateScrapCloud(transform.position, scrapCloudSize, cloudSize, scrapValue);
            }
        }
    }
}
