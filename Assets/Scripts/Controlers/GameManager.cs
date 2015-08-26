using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public MoveBoxScaler _screen;
    public PlayerStats _playerStats;
    public PlayerWeaponControler _playerWeaponControler;
    public UpgradeData _upgrades;
    public PickupManager _pickupManager;

    public static MoveBoxScaler screen;
    public static PlayerStats playerStats;
    public static PlayerWeaponControler playerWeaponControler;
    public static UpgradeData upgrades;
    public static PickupManager pickupManager;
    public void Awake()
    {

        //assigning screen size
        if (_screen != null)
            screen = _screen;
        else
            Debug.LogError("screen not assigned in inspector!");

        //assigning player stats
        if (_playerStats != null)
            playerStats = _playerStats;
        else
            Debug.LogError("playerStats not assigned in inspector!");

        //assiging player weapon controler
        if (_playerWeaponControler != null)
            playerWeaponControler = _playerWeaponControler;
        else
            Debug.LogError("PlayerWeapon Controler not assigned in inspector");

        if (_pickupManager != null)
            pickupManager = _pickupManager;
        else
            Debug.LogError("Pickupmanager was not assigned in inspector");

        Serialization.Load("upgrade", Serialization.fileTypes.binary, ref upgrades);

        _upgrades = upgrades;

        WeaponInit();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("OPEN A MEN WHEN THIS BUTTON IS PRESSED!!");
            Application.LoadLevel(0);
        }
        upgrades = _upgrades;
    }

    void WeaponInit()
    {
        //machinegun
        WeaponTable.FireRate[WeaponTable.Weapons.Machine_Gun] += upgrades.MachineGunBulletsPerSecond;
        WeaponTable.DamagePerBullet[WeaponTable.Weapons.Machine_Gun] = WeaponTable.DamagePerBullet[WeaponTable.Weapons.Machine_Gun] * Mathf.Pow(1.2f, upgrades.MachineGunDamagePerBullet);

        //shotgun
        WeaponTable.BulletsPerShot[WeaponTable.Weapons.Shotgun] += upgrades.ShotGunBulletsPerShot;
        WeaponTable.DamagePerBullet[WeaponTable.Weapons.Shotgun] = WeaponTable.DamagePerBullet[WeaponTable.Weapons.Shotgun] * Mathf.Pow(1.2f, upgrades.ShotGunDamagePerFragment);
    }
}
