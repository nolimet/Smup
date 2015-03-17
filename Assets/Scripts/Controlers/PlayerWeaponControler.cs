using UnityEngine;
using System.Collections;

public class PlayerWeaponControler : MonoBehaviour
{
    public delegate void DelegateFireWeaon(WeaponTable.Weapons weaponFired);
    public event DelegateFireWeaon onFireWeapon;

    public static PlayerWeaponControler instance;

    public static bool Firing = false;

    bool canMainShoot = true;
    public Vector2 weaponOffset;
    public WeaponTable.Weapons currentWeapon;
     

    Rigidbody2D rigi;
    // Use this for initialization
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rigi = GetComponent<Rigidbody2D>();
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
            currentWeapon = WeaponTable.Weapons.Cannon;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = WeaponTable.Weapons.Machine_Gun;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            currentWeapon = WeaponTable.Weapons.Shotgun;
    }

    void FireMain()
    {
        Firing = true;
        if (!canMainShoot) 
            return;
        if (!PlayerStats.instance.canFire(WeaponTable.EnergyUse[currentWeapon] / WeaponTable.FireRate[currentWeapon]))
            return;

        StartCoroutine(fireDelay(WeaponTable.FireRate[currentWeapon]));


        GameObject B;
        WeaponBase W;
        float angle;
        for (int i = 0; i < WeaponTable.BulletsPerShot[currentWeapon]; i++)
        {
            B = Instantiate(Resources.Load("Weapons/" + currentWeapon.ToString()), transform.position + (Vector3)weaponOffset, Quaternion.identity) as GameObject;
            W = B.GetComponent<WeaponBase>();

            angle = (Random.Range(-0.5f, 0.5f) * WeaponTable.Accuracy[currentWeapon]);

            B.transform.rotation = Quaternion.Euler(0, 0, angle);
            W.Init(getAddedVelocity(), MathHelper.AngleToVector(angle), WeaponTable.bulletSpeed[currentWeapon], WeaponTable.DamagePerBullet[currentWeapon]);
        }

        if (onFireWeapon != null)
            onFireWeapon(currentWeapon);
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
