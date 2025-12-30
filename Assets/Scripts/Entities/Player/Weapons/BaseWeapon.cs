using UnityEngine;

namespace Entities.Player.Weapons
{
    public interface IBaseWeapon
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entiy">Object that is shooting</param>
        /// <param name="weaponOffSet">of set of the weapons position</param>
        /// <param name="inherentVelocity">the velocity the bullet inherents from the shooting object</param>
        bool TryShoot(GameObject entiy, Vector3 weaponOffSet, Vector2 inherentVelocity);

        float EnergyCost { get; }
        float DelayDelta { get; }
    }
}
