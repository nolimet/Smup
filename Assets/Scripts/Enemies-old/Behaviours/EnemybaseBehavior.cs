using System.Collections;
using UnityEngine;

namespace Enemies_old.Behaviours
{
    [RequireComponent(typeof(PolygonCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Enemybase : MonoBehaviour
    {
        protected bool AppFocus = true, AttackBehaviourRunning, MoveBehaviourRunning, IsAlive;
        protected Rigidbody2D R;
        protected const int TicksPerSecond = 30;
        protected readonly float TickTimeFrag = 1f / TicksPerSecond;

        private int _health;

        protected virtual void Start() { }

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

        private void OnApplicationFocus(bool focus)
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
            IsAlive = true;
            StartBehaviours();
        }

        protected void OnDisable()
        {
            AttackBehaviourRunning = false;
            MoveBehaviourRunning = false;
        }

        protected void StartBehaviours()
        {
            if (!R)
            {
                R = GetComponent<Rigidbody2D>();
                R.gravityScale = 0;
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

        public virtual void Hit(float dmg) { }

        private void GotRemoved() { }
    }
}
