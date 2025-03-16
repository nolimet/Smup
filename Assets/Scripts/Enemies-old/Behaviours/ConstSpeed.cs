using System.Collections;
using UnityEngine;

namespace Enemies_old.Behaviours
{
    public class ConstSpeed : Enemybase
    {
        [SerializeField] private Vector2 speed;

        protected override IEnumerator EnemyMoveBehaviour()
        {
            while (IsAlive)
            {
                if (!AppFocus)
                    while (!AppFocus)
                        yield return new WaitForSeconds(TickTimeFrag);

                R.linearVelocity = speed;
                yield return new WaitForSeconds(TickTimeFrag);
            }
        }
    }
}
