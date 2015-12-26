using UnityEngine;
using System.Collections;
using System;

namespace player.Weapon
{
    public class MiniGun : BaseWeapon
    {
        public MiniGun()
        {
            fireRate = Mathf.FloorToInt((2 * Mathf.Pow(1.2f, GameManager.upgrades.MachineGunDamagePerBullet)) * 600);
            BulletDamage = 0.5f * Mathf.Pow(1.3f, GameManager.upgrades.MachineGunDamagePerBullet);
            Accuracy = 13 * Mathf.Pow(0.9f, 1);
            BulletSpeed = 2f * Mathf.Pow(1.1f, 1);

            fireDelay = 60000f / fireRate;

            if (fireDelay < 1000 / 60f)
            {
                float temp = fireDelay;
                bulletsPerShot = 1;
                while (fireDelay < 1000 / 60f)
                {
                    fireDelay += temp;
                    bulletsPerShot++;
                }
            }
            else
                bulletsPerShot = 1;
        }

        System.DateTime lastShot;
        int bulletsPerShot;
        //shots per minut;
        int fireRate;
        //number of millisecond of delay per shot
        float fireDelay;
        float Accuracy;
        float BulletSpeed;
        float BulletDamage;

        WeaponBase W;
        public void Shoot(GameObject Entiy, Vector3 weaponOffSet)
        {
            if((System.DateTime.Now-lastShot).TotalMilliseconds>= fireDelay)
            {
                lastShot = System.DateTime.Now;

                for (int i = 0; i < bulletsPerShot; i++)
                {
                    W = BulletPool.GetBullet(WeaponTable.Weapons);
                }
            }
        }
    }
}
