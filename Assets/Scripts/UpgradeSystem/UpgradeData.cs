[System.Serializable]
public class UpgradeData {

    public long UpgradeCurrency = 0;

    //Shield
    public bool UnlockedShield = false;
    public int shieldGeneratorLevel = 0;
    public int shieldCapacitorLevel = 0;
    public bool UnlockedShieldTweaker = false;

    //Hull
    public int hullUpgradeLevel = 1;
    public int armorUpgradeLevel = 1;

    //Shotgun
    public bool UnlockedShotgun = false;
    public int ShotGunBulletsPerShot = 4;
    public int ShotGunDamagePerFragment = 1;
    public bool unlockedShotgunTweaker = false;
    public int ShotgunSpreadRangeTweaker = 1;
    public int ShotgunBulletSpeedTweakerRange = 1;

    //MachineGun
    public bool UnlockedMachineGun = false;
    public int MachineGunBulletsPerSecond = 3;
    public int MachineGunDamagePerBullet = 3;
    public bool UnlockedMachineGunTweaker = false;
    public int MachineGunSpreadRangeTweaker = 1;
    public int MachineGunBulletSpeedTweakerRange = 1;
}
