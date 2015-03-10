using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Store for all the data a weapon might need
/// </summary>
public class WeaponTable
{
    /// <summary>
    /// Shots fired per second
    /// </summary>
    public static Dictionary<Weapons, int> FireRate = new Dictionary<Weapons,int>{
        {Weapons.Cannon,                20},
        {Weapons.MachineGun,            50}
    };

    /// <summary>
    /// damage dealt per bullet
    /// </summary>
    public static Dictionary<Weapons, int> DamagePerShot = new Dictionary<Weapons, int> { 
        {Weapons.Cannon,                10},
        {Weapons.MachineGun,            3}
    };

    /// <summary>
    /// flying speed per bullet
    /// </summary>
    public static Dictionary<Weapons, int> bulletSpeed = new Dictionary<Weapons, int>{
        {Weapons.Cannon,                5},
        {Weapons.MachineGun,            4}           
    };

    /// <summary>
    /// bullet spray in degrees/eulerAngles
    /// </summary>
    public static Dictionary<Weapons, int> Accuracy = new Dictionary<Weapons, int>{
        {Weapons.Cannon,                5},
        {Weapons.MachineGun,            15}
    };

    //should explain self
    public enum Weapons{
        Cannon,
        MachineGun
    }
}
