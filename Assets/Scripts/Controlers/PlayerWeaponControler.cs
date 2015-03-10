using UnityEngine;
using System.Collections;

public class PlayerWeaponControler : MonoBehaviour
{
    bool canShoot = true;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(Axis.Fire))
            Fire();
        
    }

    void Fire()
    {
        if (!canShoot)
            return;
        StartCoroutine(fireDelay(WeaponTable.FireRate[WeaponTable.Weapons.Cannon]));
        Debug.Log("fire");
    }

    IEnumerator fireDelay(float fireRate)
    {
        canShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canShoot = true;
    }
}
