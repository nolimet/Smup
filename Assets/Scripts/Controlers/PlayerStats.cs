using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    Bar Energy, Health;
    [SerializeField]
    Text CollectedScrapDisplay;

    public float currentEnergy = 0;
    public float maxEnergy = 200;

    public float currentHealth = 0;
    public float maxHealth = 100;

    public float currentShield = 0;
    public float maxShield = 0;

    void Start()
    {
        maxHealth = (GameManager.upgrades.hullUpgradeLevel + 1) * 100;

        Energy.init(maxEnergy, 2f);
        Health.init(maxHealth, 100);

        currentHealth = maxHealth;
        Health.UpdateSize(maxHealth);

        GameManager.playerWeaponControler.onFireWeapon += PlayerWeapon_onFireWeapon;
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
        Health.UpdateSize(currentHealth);
        CollectedScrapDisplay.text = "Scrap Collected: " + GameManager.pickupManager.pickedUpScrap;
    }

    public bool canFire(float energyNeeded)
    {
        if (currentEnergy >= energyNeeded)
            return true;
        return false;
    }

    public void hit(float value)
    {
        currentHealth -= value;
    }
}
