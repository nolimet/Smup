using UnityEngine;
using util;
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
    public static UpgradeData upgrades
    {
        get
        {
            if (instance._upgrades == null)
            {
                Serialization.Load("upgrade", Serialization.fileTypes.binary, ref instance._upgrades);
            }

            return instance._upgrades;
        }
    }
    public static PickupManager pickupManager;
    public static GameManager instance
    {
        get
        {
            if (_instance == null || !_instance)
                FindObjectOfType<GameManager>();

            if (_instance == null || !_instance)
                Debug.Log("INSTANCE NOT FOUND");

                return _instance;
        }
    }
    static GameManager _instance;
    public void Awake()
    {
        _instance = this;
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
        _instance = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.LoadLevel(0);
        }
    }
}
