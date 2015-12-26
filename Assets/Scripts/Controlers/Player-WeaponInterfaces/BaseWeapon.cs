using UnityEngine;
using System.Collections;
namespace player.Weapon
{
    public interface BaseWeapon
    {
        void Shoot(GameObject Entiy, Vector3 weaponOffSet);
    }
}