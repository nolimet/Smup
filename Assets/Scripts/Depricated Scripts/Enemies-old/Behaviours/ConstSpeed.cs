using System.Collections;
using UnityEngine;

namespace Depricated_Scripts.Enemies_old.Behaviours
{
    public class ConstSpeed : Enemybase
    {
        [SerializeField] private Vector2 speed;

        protected override IEnumerator EnemyMoveBehaviour()
        {
            while (IsAlive)
            {
                R.linearVelocity = speed;
                yield return new WaitForSeconds(TickTimeFrag);
            }
        }
    }
}
