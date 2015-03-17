using UnityEngine;
using System.Collections;

namespace Enemies
{
    public class ConstSpeed : Enemybase
    {
        [SerializeField]
        Vector2 speed;
        protected override IEnumerator EnemyMoveBehaviour()
        {
            while (appFocus)
            {
                r.velocity = speed;
                yield return new WaitForSeconds(TickTimeFrag);
            }
        }
    }
}