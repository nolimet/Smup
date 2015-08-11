using UnityEngine;
using UnityEngine.UI;
using util;
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
    bool canBuy(float value, bool substract)
    {
        if (upgrades.UpgradeCurrency >= value)
        {
            if (substract)
                upgrades.UpgradeCurrency -= (int)value;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Addes a new upgrade
    /// </summary>
    /// <param name="UpgradeFunc">The function that does the actual upgrading</param>
    /// <param name="StartCost">The starting cost of the upgrade</param>
    /// <param name="Mult">set to zero if only buy one. How steep the increase curve is</param>
    /// <param name="StartLevel">The level the upgrade has when the scene is loaded</param>
    /// <param name="name"> The name that will be displayed on screen for the upgrade</param>
    /// <param name="discription">The discription that the upgrade will be given on screen</param>
    void addUpgrade(Func<int, bool, bool> UpgradeFunc, float StartCost, float Mult, int StartLevel, string name, string discription)
    {
        UpgradeItem e = Instantiate(ParentUpgrade, Vector3.zero, Quaternion.identity) as UpgradeItem;
        e.transform.SetParent(ContentParent, false);
        e.gameObject.SetActive(true);
        e.Name.text = name;
        e.Discription.text = discription;

        Upgrades.Add(new UpgradeObject(UpgradeFunc, StartCost, Mult, StartLevel, e));
    }

    /// <summary>
    /// A init function for all upgrades
    /// </summary>
    void InitUpgradeFunctions()
    {
        addUpgrade(UpgradeHullLevel, 50, 1.4f, upgrades.hullUpgradeLevel, "Hull Upgrade", "Upgrading the hull allows the ship to take more hits");
        addUpgrade(UnlockShotGun, 300, 0, upgrades.UnlockedShotgun.toInt(), "Unlock Shotgun", "Unlock's the shotgun");
        addUpgrade(ShotgunBulletsPerShot, 75, 1.3f, upgrades.ShotGunBulletsPerShot, "Fragments per shotgun shot", "Increases the number of framents shot out per shot");
        addUpgrade(ShotgunDamagePerFragment, 125, 1.3f, upgrades.ShotGunDamagePerFragment, "Increase Damage per frament", "Increases the damage each frament does");
        addUpgrade(UnlockMachineGun, 700, 0, upgrades.UnlockedMachineGun.toInt(), "Unlock Machinegun", "Unlocks the Machinegun");
    }
    #endregion
    
    
    #region public functions
    #region ship
    /// <summary>
    /// Upgrade for hull/health
    /// </summary>
    /// <param name="price">The price of the upgrade</param>
    /// <param name="substract">will the upgrade be bought or only used to check if it can be bought?</param>
    /// <returns></returns>
    public bool UpgradeHullLevel(int price, bool substract)
    {
        if (canBuy(price, substract))
        {
            if (substract)
                upgrades.hullUpgradeLevel++;
            return true;
        }
        return false;
    }

    #endregion

    #region ShotGun
    public bool UnlockShotGun(int price, bool substract)
    {
        if (canBuy(price, substract) && !upgrades.UnlockedShotgun)
        {
            if (substract)
                upgrades.UnlockedShotgun = true;
            return true;
        }
        return false;
    }

    public bool ShotgunBulletsPerShot(int price, bool substract)
    {
        if (canBuy(price, substract))
        {
            if (substract)
                upgrades.ShotGunBulletsPerShot++;
            return true;
        }
        return false;
    }

    public bool ShotgunDamagePerFragment(int price, bool substract)
    {
        if (canBuy(price, substract))
        {
            if (substract)
                upgrades.ShotGunDamagePerFragment++;
            return true;
        }
        return false;
    }
    #endregion

    #region MachineGun
    public bool UnlockMachineGun(int price, bool substract)
    {
        if (canBuy(price, substract) && !upgrades.UnlockedMachineGun)
        {
            if (substract)
                upgrades.UnlockedMachineGun = true;
            return true;
        }
        return false;
    }

    #endregion
    #endregion

    [System.Serializable]
    public class UpgradeObject
    {
        UpgradeItem visual;

        Func<int, bool,bool> UpgradeFunc;

        float StartCost, Mult;
        int Level = 0;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UpgradeFunc">The function that will be called when upgrade button is clicked</param>
        /// <param name="StartCost">STarting cost</param>
        /// <param name="Mult">multipier. when zero it will only upgrade once</param>
        /// <param name="StartLevel">The starting level</param>
        /// <param name="visual">visual object. Used to assign stuff </param>
        public UpgradeObject(Func<int, bool,bool> UpgradeFunc, float StartCost, float Mult, int StartLevel, UpgradeItem visual)
        {
            this.visual = visual;

            this.UpgradeFunc = UpgradeFunc;
            this.StartCost = StartCost;
            this.Mult = Mult;
            Level = StartLevel;

            this.visual.Price.text = GetPrice().ToString();
            this.visual.Buy.onClick.AddListener(delegate { Upgrade(); });

            UpdateColour(false);
            UpdateLevel();
        }

        //function called when upgrading. called by buy button
        public void Upgrade()
        {
            UpdateColour(true);
            visual.Price.text = GetPrice().ToString();
            UpdateLevel();
        }

        //calculates the price
        private int GetPrice()
        {
            if (Mult > 0)
                return (int)(StartCost * Mathf.Pow(Mult, Level + 1));
            else
                return (int)StartCost;
        }

        //updates colour
        void UpdateColour(bool pay)
        {
            ColorBlock cb = visual.Buy.colors;

            if (UpgradeFunc(GetPrice(), pay))
            {
                cb.pressedColor = Color.green;
                if (pay)
                    Level++;
            }
            else
                cb.pressedColor = Color.red;
            visual.Buy.colors = cb;
        }

        //update value show for level
        void UpdateLevel()
        {
            if (Mult > 0)
                visual.CurrentLevel.text = "Level \n" + Level.ToString();
            else
            {
                visual.CurrentLevel.text = "Unlocked \n";
                if (Level == 0)
                    visual.CurrentLevel.text += "NO";
                else
                    visual.CurrentLevel.text += "YES";
            }
        }
    }
}
