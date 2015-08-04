using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class UpgradeMenu : MonoBehaviour
{

    [SerializeField]
    UpgradeData upgrades;

    public Text UpgradeCurrency;
    public UpgradeItem ParentUpgrade;
    public Transform ContentParent;

    public List<UpgradeObject> Upgrades;
    #region private Functions
    void Awake()
    {
        Serialization.Load("upgrade", Serialization.fileTypes.binary, ref upgrades);

        if (upgrades == null)
        {
            upgrades = new UpgradeData();
            Serialization.Save("upgrade", Serialization.fileTypes.binary, upgrades);
        }

        InitUpgradeFunctions();
    }
    void Update()
    {
        UpgradeCurrency.text = upgrades.UpgradeCurrency.ToString() + "$";
    }
    void OnDestroy()
    {
        Serialization.Save("upgrade", Serialization.fileTypes.binary, upgrades);
    }
    bool canBuy(float value)
    {
        if (upgrades.UpgradeCurrency >= value)
        {
            upgrades.UpgradeCurrency -= (int)value;
            return true;
        }
        return false;
    }

    void addUpgrade(Func<bool> UpgradeFunc, Func<int> Pricefunc, string name, string discription)
    {
        UpgradeItem e = Instantiate(ParentUpgrade, Vector3.zero, Quaternion.identity) as UpgradeItem;
        e.transform.SetParent(ContentParent, false);
        e.gameObject.SetActive(true);
        e.Name.text = name;
        e.Discription.text = discription;

        Upgrades.Add(new UpgradeObject(UpgradeFunc, Pricefunc, e));
    }
    void InitUpgradeFunctions()
    {
        addUpgrade(UpgradeHullLevel, GetHullUpgradePrice, "Hull Upgrade", "Upgrading the hull allows the ship to take more hits");
        addUpgrade(UnlockShotGun, UnlockShotGunPrice, "Unlock Shotgun", "Unlock's the shotgun");
        addUpgrade(ShotgunBulletsPerShot, ShotgunBulletPerShotPrice, "Fragments per shotgun shot", "Increases the number of framents shot out per shot");
        addUpgrade(ShotgunDamagePerFragment, ShotgunDamagePerFragmentPrice, "Increase Damage per frament", "Increases the damage each frament does");
        addUpgrade(UnlockMachineGun, UnlockMachineGunPrice, "Unlock Machinegun", "Unlocks the Machinegun");
    }
    #endregion

    #region public functions
        #region ship
        public bool UpgradeHullLevel()
        {
            if (canBuy(GetHullUpgradePrice()))
            {
                upgrades.hullUpgradeLevel++;
                return true;
            }
            return false;
        }

        public int GetHullUpgradePrice()
        {
            return 50 * (int)Mathf.Pow(1.4f, upgrades.hullUpgradeLevel);
        }
    #endregion

        #region ShotGun
    public bool UnlockShotGun()
    {
        if (canBuy(UnlockShotGunPrice()) && !upgrades.UnlockedShotgun)
        {
            upgrades.UnlockedShotgun = true;
            return true;
        }
        return false;
    }
    public int UnlockShotGunPrice()
    {
        return 300;
    }

    public bool ShotgunBulletsPerShot()
    {
        if (canBuy(ShotgunBulletPerShotPrice()))
        {
            upgrades.ShotGunBulletsPerShot++;
            return true;
        }
        return false;
    }
    public int ShotgunBulletPerShotPrice()
    {
        return (int)(75 * Mathf.Pow(1.3f, 1 + upgrades.ShotGunBulletsPerShot));
    }

    public bool ShotgunDamagePerFragment()
    {
        if (canBuy(ShotgunDamagePerFragmentPrice()))
        {
            upgrades.ShotGunDamagePerFragment++;
            return true;
        }
        return false;
    }
    public int ShotgunDamagePerFragmentPrice()
    {
        return (int)(125 * Mathf.Pow(1.3f, 1 + upgrades.ShotGunDamagePerFragment));
    }
        #endregion

        #region MachineGun
        public bool UnlockMachineGun()
        {
            if (canBuy(UnlockMachineGunPrice()) && !upgrades.UnlockedMachineGun)
            {
                upgrades.UnlockedMachineGun = true;
                return true;
            }
            return false;
        }

        public int UnlockMachineGunPrice()
        {
            return 700;
        }
        #endregion
    #endregion

    [System.Serializable]
    public class UpgradeObject
    {
        UpgradeItem visual;

        [SerializeField]
        Func<bool> UpgradeFunc;
        Func<int> Pricefunc;

        public UpgradeObject(Func<bool> UpgradeFunc, Func<int> Pricefunc, UpgradeItem visual)
        {
            this.visual = visual;

            this.UpgradeFunc = UpgradeFunc;
            this.Pricefunc = Pricefunc;

            this.visual.Price.text = Pricefunc().ToString();
            this.visual.Buy.onClick.AddListener(delegate { Upgrade(); });

            UpdateColour();
        }

        public void Upgrade()
        {
            UpdateColour();
            visual.Price.text = Pricefunc().ToString();
        }

        void UpdateColour()
        {
            ColorBlock cb = visual.Buy.colors;

            if (UpgradeFunc())
                cb.pressedColor = Color.green;
            else
                cb.pressedColor = Color.red;
            visual.Buy.colors = cb;
        }
    }
}
