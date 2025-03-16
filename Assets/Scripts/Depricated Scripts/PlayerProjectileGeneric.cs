using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Depricated_Scripts
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerProjectileGeneric : MonoBehaviour
    {
        [FormerlySerializedAs("WeaponType")] public WeaponTable.Weapons weaponType;
        protected Vector2 MoveDir;
        protected float Speed, Damage;
        protected Rigidbody2D Rigi;
        protected bool MarkedForRemove;

        /// <summary>
        /// Sets the paramaters that the bullet will use to move around
        /// </summary>
        /// <param name="moveDirNormal">The move direction clamped between 1 & -1</param>
        /// <param name="speed">How quickly it will move</param>
        /// <param name="damage">The amount of damage that will be done on impact</param>
        /// <param name="addVelo">Add Exstra velocity from for example the player or enemies</param>
        public virtual void Init(Vector2 addVelo, Vector2 moveDirNormal, float speed, float damage)
        {
            MoveDir = moveDirNormal;
            Speed = speed;
            Damage = damage;

            if (!Rigi)
                Rigi = GetComponent<Rigidbody2D>();

            Rigi.linearVelocity = MoveDir * speed + addVelo;
        }

        protected virtual void Start()
        {
            Rigi = GetComponent<Rigidbody2D>();
        }

        protected virtual void OnEnable()
        {
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color = Color.white;
            MarkedForRemove = false;
        }

        public void OnCollisionEnter2D(Collision2D coll)
        {
            if (!MarkedForRemove)
            {
                //  rigi.AddTorque(Random.Range(-10, 10));
                StartCoroutine(Remove(0.5f));
                coll.gameObject.SendMessage("hit", Damage, SendMessageOptions.DontRequireReceiver);
            }
        }

        public void OnTriggerEnter2D(Collider2D other)
        {
            if (!MarkedForRemove)
                StartCoroutine(Remove(0f));
        }

        private IEnumerator Remove(float delay)
        {
            if (!MarkedForRemove)
            {
                const float frag = 1f / 30;
                var r = GetComponent<SpriteRenderer>();
                var startColor = r.color;
                var targetColor = r.color;

                targetColor.a = 0;
                MarkedForRemove = true;

                yield return new WaitForSeconds(delay);
                GetComponent<PolygonCollider2D>().enabled = false;

                for (var i = 0; i < 30; i++)
                {
                    r.color = Color.Lerp(startColor, targetColor, frag * i);
                    yield return new WaitForEndOfFrame();
                }

                // BulletPool.RemoveBullet(this);
            }
        }
    }
}
