using System;

namespace UpgradeSystem
{
    [Serializable]
    public class UpgradeData
    {
        public long upgradeCurrency;

        //ScrapGetting
        public readonly Upgradable<int> scrapCollectionRange = new(-1, l => l, l => 900 * 1.3f);
        public readonly Upgradable<int> scrapCollectionSpeed = new(10, l => l, l => 650 * 1.3f);
        public readonly Upgradable<int> scrapConversionRate = new(-1, l => l, l => 650 * 1.5f);

        //Shield
        public readonly bool unlockedShield;
        public readonly int shieldGeneratorLevel;
        public readonly int shieldCapacitorLevel;
        public readonly bool unlockedShieldTweaker;

        //Hull
        public readonly int hullUpgradeLevel;
        public readonly int armorUpgradeLevel;

        //weaponUpgrades
        public readonly CommonWeaponUpgrade cannon;
        public readonly CommonWeaponUpgrade miniGun;
        public readonly CommonWeaponUpgrade shotgun;
        public readonly CommonWeaponUpgrade grenade;
    }

    [Serializable]
    public struct CommonWeaponUpgrade
    {
        public bool unlocked;
        public int damage;
        public int accuracy;
        public int fireRate;
        public int fragments;

        public bool unlockedTweaker;
        public float accuracyAdjustment;
        public float speedAdjustment;
        public float fireRateAdjustment;
    }
}
