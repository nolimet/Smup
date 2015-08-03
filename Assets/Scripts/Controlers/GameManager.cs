using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public MoveBoxScaler _screen;
    public PlayerStats _playerStats;
    public PlayerWeaponControler _playerWeaponControler;

    public static MoveBoxScaler screen;
    public static PlayerStats playerStats;
    public static PlayerWeaponControler playerWeaponControler;

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
    }
}
