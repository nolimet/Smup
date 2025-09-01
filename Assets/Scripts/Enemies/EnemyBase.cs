using System;
using Cysharp.Threading.Tasks;
using ObjectPools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public class EnemyBase : MonoBehaviour, IPoolElement
    {
        public string PoolId => TypeName;

        private Rigidbody2D _rigidBody2D;
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;

        [SerializeField] private float moveSpeed = 1f;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private int scrapValue;
        [SerializeField] private Vector2 scrapCloudSize = Vector2.one * 7;

        [field: SerializeField] public double Health { get; private set; }
        [field: SerializeField] public double MaxHealth { get; private set; }
        [field: SerializeField] public string TypeName { get; private set; }

        [OdinSerialize] [TypeDrawerSettings(BaseType = typeof(IMovement))]
        private Type MovementType
        {
            get => movementPattern?.GetType();
            set
            {
                if (movementPattern?.GetType() != value && value != null)
                    movementPattern = (IMovement)Activator.CreateInstance(value);
                else movementPattern = null;
            }
        }

        [SerializeReference] private IMovement movementPattern;

        [OdinSerialize] [TypeDrawerSettings(BaseType = typeof(IAttack))]
        private Type AttackType
        {
            get => attackPattern?.GetType();
            set
            {
                if (attackPattern?.GetType() != value && value != null)
                    attackPattern = (IAttack)Activator.CreateInstance(value);
                else attackPattern = null;
            }
        }

        [SerializeReference] private IAttack attackPattern;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            attackPattern = new ShootFixedIntervalAttack();
            movementPattern ??= new LinearMovement();

            movementPattern.SetTarget(gameObject);
            //attackPattern.Weapon = new WeaponInterfaces.MiniGun();
        }

        public virtual void OnSpawn()
        {
            Health = MaxHealth;
        }

        public void OnDespawn() { }

        private void Update()
        {
            attackPattern.Attack(gameObject);
            movementPattern.Move(transform.position, moveSpeed, Time.deltaTime);
        }

        public void DoDamage(double damage)
        {
            if (Health <= 0)
                return;

            Health -= damage;
            if (Health <= 0) DestroyLoop().Forget();
        }

        private async UniTaskVoid DestroyLoop()
        {
            while (_spriteRenderer.color.a > 0.01f)
            {
                var color = _spriteRenderer.color;
                color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / fadeDuration);
                _spriteRenderer.color = color;
                await UniTask.Yield();
            }

            EnemyPool.Instance.ReleaseObject(this);

            if (scrapValue > 0)
            {
                var cloudSize = Random.Range(4, 20);
                ScrapPickupPool.CreateScrapCloud(transform.position, scrapCloudSize, cloudSize, scrapValue);
            }
        }
    }
}
