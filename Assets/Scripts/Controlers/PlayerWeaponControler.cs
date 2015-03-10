using UnityEngine;
using System.Collections;

public class PlayerWeaponControler : MonoBehaviour
{
    bool canMainShoot = true;
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
        StartCoroutine(fireDelay(WeaponTable.FireRate[WeaponTable.Weapons.Cannon]));
        Debug.Log("fire");
    }

    IEnumerator fireDelay(float fireRate)
    {
        canMainShoot = false;
        yield return new WaitForSeconds(1f / fireRate);
        canMainShoot = true;
    }
}
