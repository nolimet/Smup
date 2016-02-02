using UnityEngine;
using System.Collections;
namespace player.Weapon
{
    public interface IBaseWeapon
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Entiy">Object that is shooting</param>
        /// <param name="weaponOffSet">of set of the weapons position</param>
        /// <param name="inherentVelocity">the velocity the bullet inherents from the shooting object</param>
        bool Shoot(GameObject Entiy, Vector3 weaponOffSet, Vector2 inherentVelocity);
    }
}