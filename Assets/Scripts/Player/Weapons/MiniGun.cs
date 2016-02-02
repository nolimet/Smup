using UnityEngine;
using System.Collections;

namespace player.Weapon
{
    public class MiniGun : IBaseWeapon
    {
        public MiniGun()
        {
            fireRate = Mathf.FloorToInt((3 * Mathf.Pow(1.2f, GameManager.upgrades.MiniGun.FireRate)) * 600);
            BulletDamage = 0.5f * Mathf.Pow(1.3f, GameManager.upgrades.MiniGun.Damage);
            Accuracy = 13 * Mathf.Pow(0.9f, GameManager.upgrades.MiniGun.Accuracy);
            BulletSpeed = 4f * Mathf.Pow(1.1f, 1);

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


        int _energyCost;
        public float energyCost
        {
            get
            {
                return _energyCost;
            }
        }

        WeaponBase W;
        public bool Shoot(GameObject Entiy, Vector3 weaponOffSet, Vector2 inherentVelocity)
        {
            if((System.DateTime.Now-lastShot).TotalMilliseconds>= fireDelay)
            {
                lastShot = System.DateTime.Now;
                float angle;
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    W = BulletPool.GetBullet(WeaponTable.Weapons.Machine_Gun);

                    angle = (Random.Range(-0.5f, 0.5f) * Accuracy);

                    W.transform.rotation = Quaternion.Euler(0, 0, angle);
                    W.transform.position = Entiy.transform.position + weaponOffSet;
                    W.Init(inherentVelocity, util.MathHelper.AngleToVector(angle), BulletSpeed, BulletDamage);
                }
                return true;
            }

            return false;
        }
    }
}
