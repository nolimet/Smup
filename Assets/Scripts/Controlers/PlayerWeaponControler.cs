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
        
    }

    void FireMain()
    {
        if (!canMainShoot)
            return;

        StartCoroutine(fireDelay(WeaponTable.FireRate[currentWeapon]));
        GameObject B = Instantiate(Resources.Load("Weapons/" + currentWeapon.ToString()), transform.position + (Vector3)weaponOffset, Quaternion.identity) as GameObject;
        WeaponBase W = B.GetComponent<WeaponBase>();
        W.Init(getFireAngle(),WeaponTable.bulletSpeed[currentWeapon],WeaponTable.DamagePerShot[currentWeapon]);
    }

    Vector2 getFireAngle()
    {
        const float angleOffSet = 0;
        float angle = angleOffSet + (Random.RandomRange(-0.5f, 0.5f) * WeaponTable.Accuracy[currentWeapon]);

        return MathHelper.AngleToVector(angle);
    }

    IEnumerator fireDelay(float fireRate)
    {
        canMainShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canMainShoot = true;
    }
}
