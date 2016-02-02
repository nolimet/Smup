using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public MoveBoxScaler _screen;
    public PlayerStats _playerStats;
    public PlayerWeaponControler _playerWeaponControler;
    public UpgradeData _upgrades;
    public PickupManager _pickupManager;

    public static MoveBoxScaler screen;
    public static PlayerStats playerStats;
    public static PlayerWeaponControler playerWeaponControler;
    public static UpgradeData upgrades { get { return instance._upgrades; } }
    public static PickupManager pickupManager;
    static GameManager instance;
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

        Serialization.Load("upgrade", Serialization.fileTypes.binary, ref _upgrades);
    }

    public void OnDestroy()
    {
        screen = null;
        playerStats = null;
        playerWeaponControler = null;
        pickupManager = null;
        instance = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(0);
        }
    }
}
