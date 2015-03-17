using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    public static PlayerStats instance;

    [SerializeField]
    Bar Energy;
    public float currentEnergy = 0;
    public float maxEnergy = 200;

    void Awake() { instance = this; }


    void Start()
    {
        Energy.init(maxEnergy, 2f);
        PlayerWeaponControler.instance.onFireWeapon += PlayerWeapon_onFireWeapon;
    }

    void PlayerWeapon_onFireWeapon(WeaponTable.Weapons weaponFired)
    {
        if (currentEnergy > 0)
            currentEnergy -= WeaponTable.EnergyUse[weaponFired] / WeaponTable.FireRate[weaponFired];
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (currentEnergy < maxEnergy && !PlayerWeaponControler.Firing) 
            currentEnergy += 16f * Time.deltaTime;
        
        Energy.UpdateSize(currentEnergy);
	}

    public bool canFire(float energyNeeded)
    {
        if (currentEnergy >= energyNeeded)
            return true;
        return false;
    }
}
