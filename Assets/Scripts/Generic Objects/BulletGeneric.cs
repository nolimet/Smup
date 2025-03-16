using System.Collections;
using ObjectPools;
using UnityEngine;
using UnityEngine.Serialization;
using Util;

namespace Generic_Objects
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PolygonCollider2D))]
    public class BulletGeneric : MonoBehaviour
    {
        public enum Type
        {
            Fragment,
            Bullet
        }

        [FormerlySerializedAs("WeaponType")] public Type weaponType;

        private Rigidbody2D _rigidbody;
        private SpriteRenderer _renderer;
        private PolygonCollider2D _collider;

        private int _fragmentsExplosion;
        private float _detonationTime;
        private bool _canExplode;
        private float _damage;
        private float _speed;

        public float Damage => _damage;

        protected bool MarkedForRemove;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _renderer = GetComponent<SpriteRenderer>();
            _collider = GetComponent<PolygonCollider2D>();
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
        }

        public void OnEnable()
        {
            _collider.enabled = true;
            _renderer.color = Color.white;
            MarkedForRemove = false;
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (!MarkedForRemove)
            {
                StartCoroutine(Remove(0.5f));
                coll.gameObject.SendMessage("hit", _damage, SendMessageOptions.DontRequireReceiver);

                if (_canExplode)
                    Explode();
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!MarkedForRemove)
                StartCoroutine(Remove(0f));
        }

        public void Init(float damage, float direction, float speed)
        {
            _damage = damage;

            transform.rotation = Quaternion.Euler(0, 0, direction);

            _rigidbody.linearVelocity = Common.AngleToVector(direction) * speed;

            _fragmentsExplosion = 0;

            _speed = speed;
        }

        public void Init(float damage, float direction, float speed, int fragmentsExplosion)
        {
            Init(damage, direction, speed);

            _fragmentsExplosion = fragmentsExplosion;
        }

        public void Init(float damage, float direction, float speed, int fragmentsExplosion, float explosionDelayTime, bool isTimeDelayed)
        {
            Init(damage, direction, speed, fragmentsExplosion);
            if (isTimeDelayed)
                _detonationTime = explosionDelayTime;
            else
                _detonationTime = explosionDelayTime / speed;
            _canExplode = true;
        }

        private void Explode()
        {
            if (_fragmentsExplosion <= 0)
                return;

            BulletGeneric b;
            float l = _fragmentsExplosion;
            var r = 360f / _fragmentsExplosion;
            while (_fragmentsExplosion > 0)
            {
                _fragmentsExplosion--;

                //TODO: Make Edicated Explosive bullet

                b = BulletPool.GetBullet(Type.Bullet);
                b.weaponType = Type.Fragment;
                Debug.Log(_fragmentsExplosion);
                b.transform.position = transform.position;
                b.Init(_damage, r * _fragmentsExplosion + Random.Range(-r / 2f, r / 2f), _speed);
            }
        }

        private IEnumerator Remove(float delay)
        {
            if (!MarkedForRemove)
            {
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

                weaponType = Type.Bullet;

                BulletPool.RemoveBullet(this);
            }
        }
    }
}
