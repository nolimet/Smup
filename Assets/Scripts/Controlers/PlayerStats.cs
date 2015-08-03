﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStats : MonoBehaviour {

    [SerializeField]
    Bar Energy, Health;

    public float currentEnergy = 0;
    public float maxEnergy = 200;

    public float currentHealth = 0;
    public float maxHealth = 100;

    void Start()
    {
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
