using UnityEngine;
using System.Collections;
using WeaponInterfaces;

namespace Enemy.Interfaces.Attack
{
    public interface IAttack
    {
        void Attack(GameObject Enity);
        WeaponInterfaces.Iweapon Weapon { get; set; }
    }

    public class ShootFixedIntervalAttack: IAttack
    {
        float counter;
        int fireRepeator;
        WeaponInterfaces.Iweapon weapon;

        public Iweapon Weapon
        {
            get
            {
                return weapon;
            }

            set
            {
                weapon = value;
            }
        }

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