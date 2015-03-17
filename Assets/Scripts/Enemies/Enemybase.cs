using UnityEngine;
using System.Collections;
namespace Enemies
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemybase : MonoBehaviour
    {

        protected bool appFocus;
        protected Rigidbody2D r;
        protected const int TicksPerSecond = 30;
        protected readonly float TickTimeFrag = 1f/TicksPerSecond;

        int health;

        virtual protected void Start()
        {

        }

        protected virtual IEnumerator EnemyMoveBehaviour()
        {
            //while (appFocus)
            yield return new WaitForEndOfFrame();
        }

        protected virtual IEnumerator EnemyAttackBehaviour()
        {
            // while(appFocus)

            yield return new WaitForEndOfFrame();
        }

        void OnApplicationFocus(bool focus)
        {
            if (!r)
            {
                r = GetComponent<Rigidbody2D>();
                r.gravityScale = 0;
            }
            appFocus = focus;

            StartCoroutine(EnemyMoveBehaviour());
            StartCoroutine(EnemyAttackBehaviour());
        }

        public void Hit(float dmg)
        {

        }
    }
}
