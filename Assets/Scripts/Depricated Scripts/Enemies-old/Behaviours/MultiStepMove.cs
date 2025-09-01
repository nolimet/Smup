using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies_old.Behaviours
{
    public class MultiStepMove : Enemybase
    {
        [FormerlySerializedAs("MovePatern")] [SerializeField] private Vector2[] movePatern;
        [SerializeField] private float[] duration;
        [SerializeField] private float waitBetweenPatern;

        protected override void Start()
        {
            base.Start();
            if (movePatern.Length != duration.Length)
                Debug.LogError("delay and movePatern is not same length");
        }

        protected override IEnumerator EnemyMoveBehaviour()
        {
            var l = movePatern.Length;
            Vector2 lastSpeed;
            if (!R)
                R = GetComponent<Rigidbody2D>();

            while (IsAlive)
                for (var i = 0; i < l; i++)
                {
                    lastSpeed = R.linearVelocity;
                    var k = duration[i] * TicksPerSecond;
                    for (var j = 0; j < k; j++)
                    {
                        R.linearVelocity = Vector2.Lerp(lastSpeed, movePatern[i], 1f / k * j);
                        yield return new WaitForSeconds(TickTimeFrag);
                    }

                    yield return new WaitForSeconds(waitBetweenPatern);
                }

            R.linearVelocity = Vector2.zero;
            MoveBehaviourRunning = false;
        }
    }
}
