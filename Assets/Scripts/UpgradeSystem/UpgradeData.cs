using System;

namespace UpgradeSystem
{
    [Serializable]
    public class UpgradeData
    {
        public long upgradeCurrency;

        //ScrapGetting
        public int scrapCollectionRange;
        public int scrapCollectionSpeed;
        public int scrapConversionRate;

        //Shield
        public bool unlockedShield;
        public int shieldGeneratorLevel;
        public int shieldCapacitorLevel;
        public bool unlockedShieldTweaker;

        //Hull
        public int hullUpgradeLevel;
        public int armorUpgradeLevel;

        //weaponUpgrades
        public CommonWeaponUpgrade cannon;
        public CommonWeaponUpgrade miniGun;
        public CommonWeaponUpgrade shotgun;
        public CommonWeaponUpgrade grenade;
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
