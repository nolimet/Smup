using System.Collections;
using UnityEngine;

namespace player.Weapon
{
    public class Granade : IBaseWeapon
    {
        public Granade()
        {
            //rounds per minute
            fireRate = Mathf.RoundToInt(10 * Mathf.Pow(1.3f, GameManager.upgrades.Granade.FireRate));
            //damage per bullet
            BulletDamage = 7f * Mathf.Pow(1.3f, GameManager.upgrades.Granade.Damage);
            Accuracy = 4 * Mathf.Pow(0.9f, GameManager.upgrades.Granade.Accuracy);
            BulletSpeed = 9f * Mathf.Pow(1.1f, 1);

            fragments = GameManager.upgrades.Granade.Fragments + 20;

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

            _energyCost = 50;

            // bulletsPerShot *= Mathf.RoundToInt(5*Mathf.Pow(1.2f, GameManager.upgrades.ShotGunBulletsPerShot));
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
        int fragments;
        BulletGeneric W;
        int _energyCost;
        public float energyCost
        {
            get
            {
                return _energyCost;
            }
        }

        public float delayDelta
        {
            get
            {
                return (float)(System.DateTime.Now - lastShot).TotalMilliseconds / fireDelay;
            }
        }

        public bool Shoot(GameObject Entiy, Vector3 weaponOffSet, Vector2 inherentVelocity)
        {
            if ((System.DateTime.Now - lastShot).TotalMilliseconds >= fireDelay)
            {
                lastShot = System.DateTime.Now;
                float angle;
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    W = BulletPool.GetBullet(BulletGeneric.Type.Bullet);

                    angle = (Random.Range(-0.5f, 0.5f) * Accuracy);

                    W.transform.position = Entiy.transform.position + weaponOffSet;
                    W.Init(BulletDamage, angle, BulletSpeed, fragments, 6f, false);
                }
                return true;
            }

            return false;
        }
    }
}
