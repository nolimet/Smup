using UnityEngine;
using System.Collections;
using System;

public class BulletGeneric : MonoBehaviour
{
   
}

namespace WeaponInterfaces
{
    public interface Iweapon
    {
        void Shoot(GameObject entiy, float angle);
    }

    public class MiniGun : Iweapon
    {


        public void Shoot(GameObject entiy, float angle)
        {
            throw new NotImplementedException();
        }
    }

    public class LaserCannon : Iweapon
    {
        public void Shoot(GameObject entiy, float angle)
        {
            throw new NotImplementedException();
        }
    }
}