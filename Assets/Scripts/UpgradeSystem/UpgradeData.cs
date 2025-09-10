using System;
using Sirenix.OdinInspector;

namespace UpgradeSystem
{
    [Serializable]
    public class UpgradeData
    {
        public long upgradeCurrency;

        //ScrapGetting
        [ShowInInspector] public readonly Upgradable<int> scrapCollectionRange = new(-1, l => l, l => 900 * Math.Pow(1.3, l));
        [ShowInInspector] public readonly Upgradable<int> scrapCollectionSpeed = new(10, l => l, l => 650 * Math.Pow(1.3, l));
        [ShowInInspector] public readonly Upgradable<int> scrapConversionRate = new(-1, l => l, l => 650 * Math.Pow(1.5, l));

        //Hull
        [ShowInInspector] public readonly Upgradable<int> hullUpgradeLevel = new(10, l => l, l => 50 * Math.Pow(2, l));
        [ShowInInspector] public readonly Upgradable<int> armorUpgradeLevel = new(-1, l => l, l => 100 * Math.Pow(1.4, l));

        //Shield
        [ShowInInspector] public readonly Upgradable<bool> unlockedShield = new(1, l => l > 0, _ => 1200);
        [ShowInInspector] public readonly Upgradable<int> shieldGeneratorLevel = new(-1, l => l, l => 1300 * Math.Pow(1.2, l));
        [ShowInInspector] public readonly Upgradable<int> shieldCapacitorLevel = new(-1, l => l, l => 1800 * Math.Pow(1.2f, l));
        //[ShowInInspector] public readonly Upgradable<bool> unlockedShieldTweaker;

        //weaponUpgrades
        [ShowInInspector] public readonly CommonWeaponUpgrade cannon;
        [ShowInInspector] public readonly CommonWeaponUpgrade miniGun;
        [ShowInInspector] public readonly CommonWeaponUpgrade shotgun;
        [ShowInInspector] public readonly CommonWeaponUpgrade grenade;
    }

    [Serializable]
    public struct CommonWeaponUpgrade
    {
        [ShowInInspector] public readonly Upgradable<bool> unlocked;
        [ShowInInspector] public readonly Upgradable<int> damage;
        [ShowInInspector] public readonly Upgradable<int> accuracy;
        [ShowInInspector] public readonly Upgradable<int> fireRate;
        [ShowInInspector] public readonly Upgradable<int> fragments;

        public CommonWeaponUpgrade(Upgradable<bool> unlocked, Upgradable<int> damage, Upgradable<int> accuracy, Upgradable<int> fireRate, Upgradable<int> fragments)
        {
            this.unlocked = unlocked;
            this.damage = damage;
            this.accuracy = accuracy;
            this.fireRate = fireRate;
            this.fragments = fragments;
        }
        //Not implemented yet
        /*public bool unlockedTweaker;
        public float accuracyAdjustment;
        public float speedAdjustment;
        public float fireRateAdjustment;*/
    }
}
