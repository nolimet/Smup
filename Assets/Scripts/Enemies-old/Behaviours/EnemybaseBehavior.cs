using UnityEngine;
using System.Collections;
namespace Enemies
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemybase : MonoBehaviour
    {
        protected bool appFocus = true, AttackBehaviourRunning = false, MoveBehaviourRunning = false, isAlive = false;
        protected Rigidbody2D r;
        protected const int TicksPerSecond = 30;
        protected readonly float TickTimeFrag = 1f / TicksPerSecond;

        int health;

        virtual protected void Start() { }

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

            //appFocus = focus;
            //if(!appFocus)
            //{
            //    AttackBehaviourRunning = MoveBehaviourRunning = false;
            //}
            //startBehaviours();
            // startCoroutines();
        }

        protected void OnEnable()
        {
            isAlive = true;
            startBehaviours();
        }

        protected void OnDisable()
        {
            AttackBehaviourRunning = false;
            MoveBehaviourRunning = false;
        }

        protected void startBehaviours()
        {
            if (!r)
            {
                r = GetComponent<Rigidbody2D>();
                r.gravityScale = 0;
            }
            if (!MoveBehaviourRunning)
            {
                MoveBehaviourRunning = true;
                StartCoroutine(EnemyMoveBehaviour());
            }

            if (!AttackBehaviourRunning)
            {
                AttackBehaviourRunning = true;
                StartCoroutine(EnemyAttackBehaviour());
            }
        }

        public virtual void Hit(float dmg)
        {

        }

        void GotRemoved()
        {

        }
    }
}
