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
        {Weapons.Cannon,                5},
        {Weapons.Machine_Gun,            50}
    };

    /// <summary>
    /// damage dealt per bullet
    /// </summary>
    public static Dictionary<Weapons, float> DamagePerShot = new Dictionary<Weapons, float> { 
        {Weapons.Cannon,                10},
        {Weapons.Machine_Gun,            3}
    };

    /// <summary>
    /// flying speed per bullet
    /// </summary>
    public static Dictionary<Weapons, float> bulletSpeed = new Dictionary<Weapons, float>{
        {Weapons.Cannon,                5},
        {Weapons.Machine_Gun,           10}           
    };

    /// <summary>
    /// bullet spray in degrees/eulerAngles
    /// </summary>
    public static Dictionary<Weapons, float> Accuracy = new Dictionary<Weapons, float>{
        {Weapons.Cannon,                5},
        {Weapons.Machine_Gun,            15}
    };

    //should explain self
    public enum Weapons{
        Cannon,
        Machine_Gun
    }
}
