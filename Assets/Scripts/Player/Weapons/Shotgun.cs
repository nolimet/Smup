using UnityEngine;
using System.Collections;

namespace player.Weapon
{
    public class ShotGun : IBaseWeapon
    {
        public ShotGun()
        {
            fireRate = 24;
            BulletDamage = 0.5f * Mathf.Pow(1.3f, GameManager.upgrades.Shotgun.Damage);
            Accuracy = 45 * Mathf.Pow(0.9f, GameManager.upgrades.Shotgun.Accuracy);
            BulletSpeed = 7f * Mathf.Pow(1.1f, 1);

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

            _energyCost = 30;
            bulletsPerShot *= Mathf.RoundToInt(5*Mathf.Pow(1.2f, GameManager.upgrades.Shotgun.FireRate));
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

        BulletGeneric W;
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
                    W.Init(BulletDamage, angle, BulletSpeed);
                }
                return true;
            }

            return false;
        }
    }
}
