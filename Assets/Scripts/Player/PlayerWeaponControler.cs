using UnityEngine;
using Util;
using player.Weapon;
using System.Collections;

public class PlayerWeaponControler : MonoBehaviour
{
    public delegate void DelegateFireWeaon(player.Weapon.WeaponType currentGun);
    public event DelegateFireWeaon onFireWeapon;

    public static bool Firing = false;

    public Vector2 weaponOffset;


    WeaponType _CurrentWeapon;
    public WeaponType CurrentWeapon
    {
        set
        {
            if (value != _CurrentWeapon)
            {
                switch (value)
                {
                    case WeaponType.Cannon:
                        MainWeapon = new Cannon();
                        break;
                    case WeaponType.Minigun:
                        MainWeapon = new MiniGun();
                        break;

                    case WeaponType.Shotgun:
                        MainWeapon = new ShotGun();
                        break;

                    case WeaponType.Granade:
                        MainWeapon = new Granade();
                        break;
                }
            }

            _CurrentWeapon = value;
        }

        get
        {
            return _CurrentWeapon;
        }
    }
    public IBaseWeapon MainWeapon;


    Rigidbody2D rigi;
    // Use this for initialization

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
        MainWeapon = new Cannon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(Axis.Fire))
            FireMain();
        else
            Firing = false;

        SwitchWeapon();
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentWeapon = WeaponType.Cannon;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (GameManager.upgrades.MiniGun.Unlocked)
                CurrentWeapon = WeaponType.Minigun;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (GameManager.upgrades.Shotgun.Unlocked)
                CurrentWeapon = WeaponType.Shotgun;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (GameManager.upgrades.Granade.Unlocked)
                CurrentWeapon = WeaponType.Granade;
        }
    }

    void FireMain()
    {
        if (!GameManager.playerStats.canFire(MainWeapon.energyCost))
            return;

        if(MainWeapon.Shoot(gameObject, weaponOffset, getAddedVelocity()))
        {
            GameManager.playerStats.RemoveEnergy(MainWeapon.energyCost);
            onFireWeapon?.Invoke(_CurrentWeapon);
        }

        
    }

    Vector2 getAddedVelocity()
    {
        Vector2 output;

        if (rigi.velocity.x > 0)
            output = new Vector2(rigi.velocity.x, 0f);
        else
            output = Vector2.zero;
        return output;
    }
}
