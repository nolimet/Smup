[System.Serializable]
public class UpgradeData {

    public long UpgradeCurrency = 0;
    #region Ship-Upgrades
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
    #endregion

    //weaponUpgrades
    public CommonWeaponUpgrade Cannon, MiniGun, Shotgun;

    [System.Serializable]
    public struct CommonWeaponUpgrade
    {
        public bool Unlocked;
        public int Damage, Accuracy, FireRate;

        public bool UnlockedTweaker;
        public float AccuracyAdjustment, SpeedAdjustment;
    }
}
