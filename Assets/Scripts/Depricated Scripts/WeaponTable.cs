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
        {Weapons.Cannon,                 8},
        {Weapons.Machine_Gun,            50},
        {Weapons.Shotgun,                2}
    };

    /// <summary>
    /// damage dealt per bullet
    /// </summary>
    public static Dictionary<Weapons, float> DamagePerBullet = new Dictionary<Weapons, float> { 
        {Weapons.Cannon,                10},
        {Weapons.Machine_Gun,            3},
        {Weapons.Shotgun,               5}
    };

    /// <summary>
    /// flying speed per bullet
    /// </summary>
    public static Dictionary<Weapons, float> bulletSpeed = new Dictionary<Weapons, float>{
        {Weapons.Cannon,                9},
        {Weapons.Machine_Gun,           12},
        {Weapons.Shotgun,               12}
    };

    /// <summary>
    /// bullet spray in degrees/eulerAngles
    /// </summary>
    public static Dictionary<Weapons, float> Accuracy = new Dictionary<Weapons, float>{
        {Weapons.Cannon,                5},
        {Weapons.Machine_Gun,            15},
        {Weapons.Shotgun,                40}
    };

    //energy Use Per Second
    public static Dictionary<Weapons, float> EnergyUse = new Dictionary<Weapons, float>{
        {Weapons.Cannon,                10f},
        {Weapons.Machine_Gun,           5f},
        {Weapons.Shotgun,               20f}
    };

    public static Dictionary<Weapons, int> BulletsPerShot = new Dictionary<Weapons, int>{
        {Weapons.Cannon,                1},
        {Weapons.Machine_Gun,           2},
        {Weapons.Shotgun,               20}
    };
    //should explain self
    public enum Weapons{
        Cannon,
        Machine_Gun,
        Shotgun
    }
}
