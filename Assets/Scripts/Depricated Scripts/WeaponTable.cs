using System.Collections.Generic;

namespace Depricated_Scripts
{
    /// <summary>
    /// Store for all the data a weapon might need
    /// </summary>
    public class WeaponTable
    {
        /// <summary>
        /// Shots fired per second
        /// </summary>
        public static Dictionary<Weapons, int> FireRate = new()
        {
            { Weapons.Cannon, 8 },
            { Weapons.MachineGun, 50 },
            { Weapons.Shotgun, 2 }
        };

        /// <summary>
        /// damage dealt per bullet
        /// </summary>
        public static Dictionary<Weapons, float> DamagePerBullet = new()
        {
            { Weapons.Cannon, 10 },
            { Weapons.MachineGun, 3 },
            { Weapons.Shotgun, 5 }
        };

        /// <summary>
        /// flying speed per bullet
        /// </summary>
        public static Dictionary<Weapons, float> BulletSpeed = new()
        {
            { Weapons.Cannon, 9 },
            { Weapons.MachineGun, 12 },
            { Weapons.Shotgun, 12 }
        };

        /// <summary>
        /// bullet spray in degrees/eulerAngles
        /// </summary>
        public static Dictionary<Weapons, float> Accuracy = new()
        {
            { Weapons.Cannon, 5 },
            { Weapons.MachineGun, 15 },
            { Weapons.Shotgun, 40 }
        };

        //energy Use Per Second
        public static Dictionary<Weapons, float> EnergyUse = new()
        {
            { Weapons.Cannon, 10f },
            { Weapons.MachineGun, 5f },
            { Weapons.Shotgun, 20f }
        };

        public static Dictionary<Weapons, int> BulletsPerShot = new()
        {
            { Weapons.Cannon, 1 },
            { Weapons.MachineGun, 2 },
            { Weapons.Shotgun, 20 }
        };

        //should explain self
        public enum Weapons
        {
            Cannon,
            MachineGun,
            Shotgun
        }
    }
}
