using UnityEngine;
using System.Collections;

namespace Enemies
{
    public class MultiStepMove : Enemybase
    {

        [SerializeField] Vector2[] MovePatern;
        [SerializeField]
        float[] duration;
        [SerializeField]
        float waitBetweenPatern;

        protected override void Start()
        {
            base.Start();
            if (MovePatern.Length != duration.Length)
                Debug.LogError("delay and movePatern is not same length");
        }
        protected override IEnumerator EnemyMoveBehaviour()
        {
            int l = MovePatern.Length;
            Vector2 lastSpeed;
            while (appFocus)
            {
                for (int i = 0; i < l; i++)
                {
                    lastSpeed = r.velocity;
                    float k = duration[i] * TicksPerSecond;
                    for (int j = 0; j < k; j++)
                    {
                        r.velocity = Vector2.Lerp(lastSpeed, MovePatern[i], (1f/k) * j) ;
                        yield return new WaitForSeconds(TickTimeFrag);
                    }
                    yield return new WaitForSeconds(waitBetweenPatern);
                }
            }

            r.velocity = Vector2.zero;
        }

    }
}
