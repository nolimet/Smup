using UnityEngine;
using UnityEngine.UI;
using util;
using System.Collections;
using System.Collections.Generic;
using System;

public class UpgradeMenu : MonoBehaviour
{

    [SerializeField]
    public UpgradeData upgrades;

    public Text UpgradeCurrency;
    public UpgradeItem ParentUpgrade;
    public Transform ContentParent;

    public List<UpgradeObject> Upgrades;

    private static UpgradeMenu instance;
    public static UpgradeData upgradeData { get{ return instance.upgrades; } }
    #region private Functions
    void Awake()
    {
        if (!instance)
            instance = this;
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
        if (!instance)
            instance = this;

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
    /// Addes a new upgrade using the new system. It gets a value by its name and edit's it that way.
    /// This way it does not require a function for every upgrade making it much more effecient.
    /// </summary>
    /// <param name="refrence">The name of the varible as it's named in UpgradeData.cs</param>
    /// <param name="StartCost">The intal cost of the upgrade</param>
    /// <param name="Mult">set to zero it value is a bool. </param>
    /// <param name="name"> The name that will be displayed on screen for the upgrade</param>
    /// <param name="discription">The discription that the upgrade will be given on screen</param>
    /// <param name="MaxLevel">The maxium level of the upgrade. Leave it to default when wanting to disable</param>
    void addUpgrade(string refrence, float StartCost, float Mult, string name, string discription, int MaxLevel =-1)
    {
        UpgradeItem e = Instantiate(ParentUpgrade, Vector3.zero, Quaternion.identity) as UpgradeItem;
        e.transform.SetParent(ContentParent, false);
        e.gameObject.SetActive(true);
        e.Name.text = name;
        e.Discription.text = discription;

        Upgrades.Add(new UpgradeObject(refrence, StartCost,MaxLevel, Mult, e));
    }

    /// <summary>
    /// A init function for all upgrades
    /// </summary>
    void InitUpgradeFunctions()
    {
       /* addUpgrade(UpgradeHullLevel, 50, 1.4f, upgrades.hullUpgradeLevel, "Hull Upgrade", "Upgrading the hull allows the ship to take more hits");
        addUpgrade(UnlockShotGun, 300, 0, upgrades.UnlockedShotgun.toInt(), "Unlock Shotgun", "Unlock's the shotgun");
        addUpgrade(ShotgunBulletsPerShot, 75, 1.3f, upgrades.ShotGunBulletsPerShot, "Fragments per shotgun shot", "Increases the number of framents shot out per shot");
        addUpgrade(ShotgunDamagePerFragment, 125, 1.3f, upgrades.ShotGunDamagePerFragment, "Increase Damage per frament", "Increases the damage each frament does");
        addUpgrade(UnlockMachineGun, 700, 0, upgrades.UnlockedMachineGun.toInt(), "Unlock Machinegun", "Unlocks the Machinegun");*/

        //addUpgrade("UnlockedShotgunTweaker", 100, 0, "A test", "TESTING");
        addUpgrade("hullUpgradeLevel", 50, 1.4f, "Hull Upgrade", "Upgrading the hull allows the ship to take more hits");
        addUpgrade("UnlockedShotgun", 300, 0, "Fragments per shotgun shot", "Increases the number of framents shot out per shot");
        addUpgrade("ShotGunBulletsPerShot", 75,1.3f, "Fragments per shotgun shot", "Increases the number of framents shot out per shot",6);
        addUpgrade("ShotGunDamagePerFragment",125,1.3f, "Damage per frament", "Increases the damage each frament does");
        addUpgrade("UnlockedMachineGun",700,0, "Unlock Machinegun", "Unlocks the Machinegun");
        addUpgrade("MachineGunBulletsPerSecond", 250, 1.2f, "Firerate MachineGun", "Increases number of bullets shot out by the machine gun every second",20);
        addUpgrade("MachineGunDamagePerBullet", 400, 1.1f, "Damage per bullet MachineGun", "Increases the damage each bullet of the machine gun does");

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

        int Level = 0, MaxLevel =-1;
        string refrence;
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

        public UpgradeObject (string refrence, float StartCost,int MaxLevel ,float Mult, UpgradeItem visual)
        {

            var temp = upgradeData.GetType().GetField(refrence).GetValue(upgradeData);
            Debug.Log(refrence);
            if (Mult > 0)
            {
                Level = (int)temp;
            }
            else
            {
                bool b = (bool)temp;
                Level = b.toInt();
            }
            
            this.StartCost = StartCost;
            this.Mult = Mult;
            this.refrence = refrence;
            this.visual = visual;
            this.MaxLevel = MaxLevel;

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

            if ((UpgradeFunc != null && UpgradeFunc(GetPrice(), pay)) || canBuy(pay))
            {
                cb.pressedColor = Color.green;
            }
            else
                cb.pressedColor = Color.red;
            visual.Buy.colors = cb;
        }

        //update value show for level
        void UpdateLevel()
        {
            if (Mult > 0)
            {
                visual.CurrentLevel.text = "Level \n" + Level.ToString();
                if (MaxLevel > 0)
                    visual.CurrentLevel.text += "/" + MaxLevel.ToString();
            }
            else
            {
                visual.CurrentLevel.text = "Unlocked \n";
                if (Level == 0)
                    visual.CurrentLevel.text += "NO";
                else
                    visual.CurrentLevel.text += "YES";
            }
        }

        

        bool canBuy(bool substract)
        {
            if (upgradeData.UpgradeCurrency >= GetPrice() && (MaxLevel<0 || MaxLevel>0 && Level < MaxLevel) && (Mult>0 || Mult<=0 && Level<=0))
            {
                if (substract)
                {                 
                    var dat = upgradeData.GetType().GetField(refrence).GetValue(upgradeData);
                    if (Mult > 0)
                    {

                        if (MaxLevel > 0 && Level >= MaxLevel)
                            return false;

                        upgradeData.UpgradeCurrency -= GetPrice();
                        int f;

                        f = (int)dat;
                        Level++;
                        f = Level;

                        upgradeData.GetType().GetField(refrence).SetValue(upgradeData, f);
                    }
                    else
                    {
                        bool f;
                        f = (bool)dat;
                        if (f)
                            return false;

                        upgradeData.UpgradeCurrency -= GetPrice();

                        f = true;
                        Level = 1;
                        upgradeData.GetType().GetField(refrence).SetValue(upgradeData, f);

                    }
                    
                }
                return true;
            }
            return false;
        }
    }
}
