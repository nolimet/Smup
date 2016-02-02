﻿[System.Serializable]
public class UpgradeData {

    public long UpgradeCurrency = 0;

    //ScrapGetting
    public int ScrapCollectionRange = 0;
    public int ScrapCollectionSpeed = 0;
    public int ScrapConversionRate = 0;

    //Shield
    public bool UnlockedShield = false;
    public int shieldGeneratorLevel = 0;
    public int shieldCapacitorLevel = 0;
    public bool UnlockedShieldTweaker = false;

    //Hull
    public int hullUpgradeLevel = 0;
    public int armorUpgradeLevel = 0;

    //Shotgun
    public bool UnlockedShotgun = false;
    public int ShotGunBulletsPerShot = 0;
    public int ShotGunDamagePerFragment = 0;

    public bool UnlockedShotgunTweaker = false;
    public int ShotgunSpreadRangeTweaker = 0;
    public int ShotgunBulletSpeedTweakerRange = 0;

    //MachineGun
    public bool UnlockedMachineGun = false;
    public int MachineGunBulletsPerSecond = 0;
    public int MachineGunDamagePerBullet = 0;
    public int MachineGunSpread = 0;

    public bool UnlockedMachineGunTweaker = false;
    public int MachineGunSpreadRangeTweaker = 0;
    public int MachineGunBulletSpeedTweakerRange = 0;

    public commonUpgradeDat Cannon, MiniGun, Shotgun;

    [System.Serializable]
    public struct commonUpgradeDat
    {
        public bool Unlocked;
        public int Damage, Accuracy, FireRate;

        public bool UnlockedTweaker;
        public float AccuracyAdjustment, SpeedAdjustment;
    }
}
