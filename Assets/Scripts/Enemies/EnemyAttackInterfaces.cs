using UnityEngine;
using System.Collections;

namespace Enemy.Interfaces.Attack
{
    public interface IAttack
    {
        void Attack(GameObject Enity);
    }

    public class ShootFixedIntervalAttack: IAttack
    {
        float counter;
        int fireRepeator;

        public void Attack(GameObject Enity)
        {
            if(fireRepeator <=0 && counter>=5)
            {
                counter = 0f;
                fireRepeator = 5;
            }
            else if (fireRepeator > 0 && counter >= 0.3f)
            {
                counter = 0f;
                fireRepeator--;
            }

            counter += Time.deltaTime;
        }
    }
}