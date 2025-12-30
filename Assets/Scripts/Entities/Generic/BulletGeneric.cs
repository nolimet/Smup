using System.Collections;
using Entities.Interfaces;
using Pools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Util;
using Random = UnityEngine.Random;

namespace Entities.Generic
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
    public class BulletGeneric : MonoBehaviour, IPoolElement
    {
        public enum BulletType
        {
            Fragment,
            Bullet
        }

        [FormerlySerializedAs("WeaponType")] public BulletType weaponType;
        public string PoolId { get; private set; }
        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        private PolygonCollider2D _collider;

        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] private int _targetLayer;
        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] private int _fragmentsExplosion;
        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] private float _detonationTime;
        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] private bool _canExplode;
        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] private float _damage;
        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] private float _speed;

        public float Damage => _damage;

        [ShowInInspector] [Sirenix.OdinInspector.ReadOnly] protected bool MarkedForRemove;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<PolygonCollider2D>();

            PoolId = weaponType.ToString();
        }

        private void Update()
        {
            if (_canExplode)
            {
                _detonationTime -= Time.deltaTime;
                if (_detonationTime <= 0)
                {
                    Explode();
                    StartCoroutine(Remove(0f));
                    _canExplode = false;
                }
            }

            if (!MarkedForRemove && _rigidbody.linearVelocity.magnitude < 0.1f)
            {
                StartCoroutine(Remove(0f));
                MarkedForRemove = true;
            }
        }

        public void OnSpawn()
        {
            _collider.enabled = true;
            _renderer.color = Color.white;
            MarkedForRemove = false;
        }

        public void OnDespawn() { }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (MarkedForRemove || coll.gameObject.layer != _targetLayer || coll.collider.isTrigger) return;
            var damageAble = coll.gameObject.GetComponent<IDamageAble>();
            if (damageAble != null)
            {
                damageAble.ReceiveDamage(_damage);
                StartCoroutine(Remove(0.5f));

                if (_canExplode) Explode();
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Wall")) BulletPool.Instance.ReleaseObject(this);
        }

        public void Init(float damage, float direction, float speed, int targetLayer, int? fragmentsExplosion = null, float? explosionDelayTime = null, bool? isTimeDelayed = null)
        {
            _damage = damage;
            _targetLayer = targetLayer;
            transform.rotation = Quaternion.Euler(0, 0, direction);
            _rigidbody.linearVelocity = Common.AngleToVector(direction) * speed;
            _speed = speed;

            _fragmentsExplosion = fragmentsExplosion ?? 0;
            if (explosionDelayTime is not null && isTimeDelayed is not null)
            {
                if (isTimeDelayed.Value)
                    _detonationTime = explosionDelayTime.Value;
                else
                    _detonationTime = explosionDelayTime.Value / speed;
                _canExplode = true;
            }
        }

        private void Explode()
        {
            if (_fragmentsExplosion <= 0) return;

            var radiusStep = 360f / _fragmentsExplosion;
            while (_fragmentsExplosion > 0)
            {
                _fragmentsExplosion--;

                //TODO: Make Dedicated Explosive bullet

                var fragment = BulletPool.Instance.GetObject(BulletType.Fragment);
                fragment.transform.position = transform.position;
                fragment.Init(_damage, radiusStep * _fragmentsExplosion + Random.Range(-radiusStep / 2f, radiusStep / 2f), _speed, _targetLayer);
            }
        }

        private IEnumerator Remove(float delay)
        {
            if (MarkedForRemove) yield break;

            const float frag = 1f / 30;
            var startColor = _renderer.color;
            var targetColor = _renderer.color;

            targetColor.a = 0;
            MarkedForRemove = true;

            yield return new WaitForSeconds(delay);
            _collider.enabled = false;

            for (var i = 0; i < 30; i++)
            {
                _renderer.color = Color.Lerp(startColor, targetColor, frag * i);
                yield return new WaitForEndOfFrame();
            }

            BulletPool.Instance.ReleaseObject(this);
        }
    }
}
