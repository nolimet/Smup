using Cysharp.Threading.Tasks;
using Data;
using Entities.Enemies.Interfaces;
using Entities.Enemies.Movement;
using Entities.Interfaces;
using Pools;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Entities.Enemies
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public sealed class Enemy : MonoBehaviour, IPoolElement, IDamageAble
    {
        public string PoolId => TypeName;

        private Rigidbody2D _rigidBody2D;
        private Collider2D _collider2D;
        private SpriteRenderer _spriteRenderer;

        [field: SerializeField] public string TypeName { get; private set; }

        public Overrideable<float> moveSpeed = 1f;
        public Overrideable<float> fadeDuration = 1f;
        public Overrideable<int> scrapValue;
        public Overrideable<Vector2> scrapCloudSize = Vector2.one * 7;
        public Overrideable<double> contactDamage;

        [field: ReadOnly] [field: ShowInInspector] public double Health { get; private set; }
        [field: SerializeField] public Overrideable<double> MaxHealth { get; private set; }

        [SerializeReference] private IMovement movementPattern;
        [SerializeReference] private IAttack attackPattern;

        public IMovement MovementPattern => movementPattern;

        public IAttack AttackPattern => attackPattern;

        private void Awake()
        {
            _rigidBody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            movementPattern ??= new LinearMovement();
            movementPattern.SetTarget(gameObject);

            if (movementPattern is IKillAtEnd killAtEnd)
                killAtEnd.AtEnd += () => Kill(false);
        }

        private void Kill(bool doReward)
        {
            if (enabled)
                DestroyLoop().Forget();
            enabled = false;
            return;

            async UniTaskVoid DestroyLoop()
            {
                _collider2D.enabled = false;

                while (_spriteRenderer.color.a > 0.01f)
                {
                    var color = _spriteRenderer.color;
                    color.a = Mathf.MoveTowards(color.a, 0f, Time.deltaTime / fadeDuration);
                    _spriteRenderer.color = color;
                    await UniTask.Yield();
                }

                EnemyPool.Instance.ReleaseObject(this);

                if (doReward && scrapValue > 0)
                {
                    var cloudSize = Random.Range(4, 20);
                    ScrapPickupPool.CreateScrapCloud(transform.position, scrapCloudSize, cloudSize, scrapValue);
                }
            }
        }

        public void OnSpawn()
        {
            Health = MaxHealth;
            _collider2D.enabled = true;
            var color = _spriteRenderer.color;
            color.a = 1;
            _spriteRenderer.color = color;
        }

        public void OnDespawn() { }

        private void Update()
        {
            movementPattern.Move(transform.position, moveSpeed, Time.deltaTime);
            attackPattern?.Attack(transform.position, _rigidBody2D.linearVelocity);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (Health <= double.Epsilon) return;

            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                var damageAble = other.gameObject.GetComponent<IDamageAble>();
                if (damageAble != null)
                {
                    damageAble.ReceiveDamage(contactDamage);
                    Kill(false);
                    Health = 0;
                }
            }

            if (other.collider.CompareTag("Enemy Kill Plane")) Kill(false);
        }

        public void ReceiveDamage(double damage)
        {
            if (Health <= 0) return;

            Health -= damage;
            if (Health <= 0) Kill(true);
        }
    }
}
