using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WeaponTable
{
    public static Dictionary<Weapons, int> FireRate = new Dictionary<Weapons,int>{
        {Weapons.Cannon,                20},
        {Weapons.MachineGun,            50}
    };

    public static Dictionary<Weapons, int> DamagePerShot = new Dictionary<Weapons, int> { 
        {Weapons.Cannon,                10},
        {Weapons.MachineGun,            3}
    };

    public static Dictionary<Weapons, int> bulletSpeed = new Dictionary<Weapons, int>{
        {Weapons.Cannon,                5},
        {Weapons.MachineGun,            4}           
    };

    public enum Weapons{
        Cannon,
        MachineGun
    }
}
