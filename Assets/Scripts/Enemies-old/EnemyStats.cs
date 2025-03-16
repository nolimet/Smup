using System.Collections;
using ObjectPools;
using UnityEngine;

namespace Enemies_old
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class EnemyStats : MonoBehaviour
    {
        public enum Type
        {
            Shooter = 89,
            WaveMover = 90,

            SlowDownSpeedUp = 91
            //ToDo add More types
        }

        protected readonly Bounds MaxLimits = new(Vector3.zero, new Vector3(100, 100, 1));
        [SerializeField] private float health = 10, maxHealth = 10;
        public Type type;
        private bool _markedForRemove;

        public virtual void Hit(float dmg)
        {
            health -= dmg;
            if (health <= 0 && !_markedForRemove)
                StartCoroutine(Remove(0f, true));
        }

        public void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "backbullet Remover")
                StartCoroutine(Remove(0f, false));
        }

        protected virtual void Update()
        {
            if (!_markedForRemove)
                if (!MaxLimits.Contains(transform.position))
                    Remove(0, false);
        }

        protected virtual void OnEnable()
        {
            GetComponent<Rigidbody2D>().isKinematic = false;
            GetComponent<PolygonCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().color = Color.white;
            _markedForRemove = false;
            health = maxHealth;
        }

        private IEnumerator Remove(float delay, bool createPickups)
        {
            if (!_markedForRemove)
            {
                GetComponent<PolygonCollider2D>().enabled = false;

                const float frag = 1f / 30;
                var r = GetComponent<SpriteRenderer>();
                var startColor = r.color;
                var targetColor = r.color;

                targetColor.a = 0;
                _markedForRemove = true;

                yield return new WaitForSeconds(delay);

                for (var i = 0; i < 30; i++)
                {
                    r.color = Color.Lerp(startColor, targetColor, frag * i);
                    yield return new WaitForEndOfFrame();
                }

                GetComponent<Rigidbody2D>().isKinematic = true;
                SendMessage("GotRemoved");
                //ToDo add make scrap and count value dynamic and based on difficlutly of the enemy
                if (createPickups)
                    ScrapPickupPool.CreateScrapCloud(transform.position, new Vector2(7, 7), 5, 20);
                EnemyPool.RemoveEnemy(this);
            }
        }
    }
}
