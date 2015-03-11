using UnityEngine;
using System.Collections;

public class PlayerWeaponControler : MonoBehaviour
{
    bool canMainShoot = true;
    public Vector2 weaponOffset;
    public WeaponTable.Weapons currentWeapon;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(Axis.Fire))
            FireMain();
        if (Input.anyKeyDown)
            SwitchWeapon();
    }

    void SwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentWeapon = WeaponTable.Weapons.Cannon;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            currentWeapon = WeaponTable.Weapons.Machine_Gun;
    }

    void FireMain()
    {
        if (!canMainShoot)
            return;

        StartCoroutine(fireDelay(WeaponTable.FireRate[currentWeapon]));
        GameObject B = Instantiate(Resources.Load("Weapons/" + currentWeapon.ToString()), transform.position + (Vector3)weaponOffset, Quaternion.identity) as GameObject;
        WeaponBase W = B.GetComponent<WeaponBase>();
        float angle = (Random.RandomRange(-0.5f, 0.5f) * WeaponTable.Accuracy[currentWeapon]);

        B.transform.rotation = Quaternion.Euler(0, 0, angle);
        W.Init(MathHelper.AngleToVector(angle), WeaponTable.bulletSpeed[currentWeapon], WeaponTable.DamagePerShot[currentWeapon]);
    }

    IEnumerator fireDelay(float fireRate)
    {
        canMainShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canMainShoot = true;
    }
}
