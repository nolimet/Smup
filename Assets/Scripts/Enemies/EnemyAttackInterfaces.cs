using UnityEngine;
using System.Collections;

namespace interfaces.Attack
{
    public interface IAttack
    {
        void Attack(GameObject Enity);
    }

    public class ShootFixedIntervalAttack: IAttack
    {
        float counter;
        int fireRepeator;
        WeaponInterfaces.Iweapon weapon;

        public void Attack(GameObject Enity)
        {
            if(fireRepeator <=0 && counter>=5)
            {
                counter = 0f;
                fireRepeator = 5;
            }
            else if (fireRepeator >= 0 && counter >= 0.3f)
            {
                counter = 0f;
                weapon.Shoot(Enity, Random.Range(-5f, 5f));
            }

            counter += Time.deltaTime;
        }
    }
}