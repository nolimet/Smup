using UnityEngine;
using util;
using player.Weapon;
using System.Collections;

public class PlayerWeaponControler : MonoBehaviour
{
    public delegate void DelegateFireWeaon(WeaponTypes);
    public event DelegateFireWeaon onFireWeapon;

    public static bool Firing = false;

    bool canMainShoot = true;
    public Vector2 weaponOffset;


    WeaponType _CurrentWeapon;
    public WeaponType CurrentWeapon
    {
        set
        {
            if(value != _CurrentWeapon)
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
        if (Input.anyKeyDown)
            SwitchWeapon();
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            CurrentWeapon = WeaponType.Cannon;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            CurrentWeapon = WeaponType.Minigun;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            CurrentWeapon = WeaponType.Shotgun;
    }

    void FireMain()
    {
        Firing = true;
        if (!canMainShoot) 
            return;
        if (!GameManager.playerStats.canFire(WeaponTable.EnergyUse[currentWeapon] / WeaponTable.FireRate[currentWeapon]))
            return;

        StartCoroutine(fireDelay(WeaponTable.FireRate[currentWeapon]));


        WeaponBase W;
        float angle;
        //Debug.Log(currentWeapon.ToString() + ": " + WeaponTable.BulletsPerShot[currentWeapon].ToString());
        for (int i = 0; i < WeaponTable.BulletsPerShot[currentWeapon]; i++)
        {
            //B = Instantiate(Resources.Load("Weapons/" + currentWeapon.ToString()), transform.position + (Vector3)weaponOffset, Quaternion.identity) as GameObject;
            W = BulletPool.GetBullet(currentWeapon);
            W.transform.position = transform.position + (Vector3)weaponOffset;
            angle = (Random.Range(-0.5f, 0.5f) * WeaponTable.Accuracy[currentWeapon]);

            W.transform.rotation = Quaternion.Euler(0, 0, angle);
            W.Init(getAddedVelocity(), MathHelper.AngleToVector(angle), WeaponTable.bulletSpeed[currentWeapon], WeaponTable.DamagePerBullet[currentWeapon]);
        }

        if (onFireWeapon != null)
            onFireWeapon(_CurrentWeapon);
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

    IEnumerator fireDelay(float fireRate)
    {
        canMainShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canMainShoot = true;
    }
}
