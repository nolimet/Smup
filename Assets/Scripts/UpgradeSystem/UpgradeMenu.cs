﻿using UnityEngine;
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
    public UpgradeCatagory parentCatagory;
    public Transform ContentParent;

    public List<UpgradeObject> Upgrades;
    public List<CatagoryObject> cats;
    CatagoryObject currentCatagory;

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
        ParentUpgrade.gameObject.SetActive(false);
        parentCatagory.gameObject.SetActive(false);
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
    /// A init function for all upgrades
    /// </summary>
    void InitUpgradeFunctions()
    {

        //scrapCollection
        addCatagory("Scrap Collection", "Enchance your scrap collection ability");
        addUpgrade("","ScrapConversionRate", 900, 1.3f, "Scrap sell rate", "Improves for how much you sell each piece of scrap");
        addUpgrade("", "ScrapCollectionRange", 650, 1.3f, "Scrap Collection Range", "Makes the collection range bigger", 10);
        addUpgrade("", "ScrapCollectionSpeed", 650, 1.5f, "Scrap Collection Speed", "Improves how fast the scrap moves towards you", 10);

        //Hull
        addCatagory("Ship Upgrades", "Upgrades that make your ship strong, faster and generaly better");
        addUpgrade("", "hullUpgradeLevel", 50, 1.8f, "Hull Upgrade", "Upgrading the hull allows the ship to take more hits");
        addUpgrade("", "armorUpgradeLevel", 100, 1.4f, "Armor Upgrade", "Upgrades the ammount of armor the ship has");
        //Shield
        addUpgrade("", "UnlockedShield", 1200, 0, "Unlock Shield", "Unlocks the shield");
        addUpgrade("", "shieldGeneratorLevel", 1300, 1.2f, "Shield Regeneration Speed", "Increases the regeneration speed of the shield");
        addUpgrade("", "shieldCapacitorLevel", 1800, 1.2f, "Shield capacity upgrade", "Increases the capacity of the shield");

        //shotgun
        addCatagory("Shotgun Upgrades", " Upgrades for your shotgun");
        addUpgrade("Shotgun", "Unlocked", 750, 0, "Unlocks Shotgun", "Unlocks the Shotgun");
        addUpgrade("Shotgun", "Accuracy", 1425, 1.8f, "Increase Accuracy", "Decreases the spread of the shotgun");
        addUpgrade("Shotgun", "Damage", 1250, 1.3f, "Damage per frament", "Increases the damage each fragment does");
        addUpgrade("Shotgun", "FireRate", 750, 1.3f, "Fragments per Shotgun shot", "Increases the number of fragments shot out per shot", 6);

        //machineGun
        addCatagory("Machinegun Upgrades", "Upgrades for your machinegun");
        addUpgrade("MiniGun", "Unlocked", 700, 0, "Unlock Machinegun", "Unlocks the Machinegun");
        addUpgrade("MiniGun", "Accuracy", 3000, 1.3f, "Increase Accuracy", "Increases Accuracy of the Machinegun");
        addUpgrade("MiniGun", "Damage", 4000, 1.1f, "Increase Damage", "Increases the damage each bullet of the machine gun does");
        addUpgrade("MiniGun", "FireRate", 2500, 1.2f, "Increase Firerate", "Increases number of bullets shot out by the machine gun every second", 20);

        //Cannon
        addCatagory("Cannon Upgrades", "Upgrades for your main Cannon");
        addUpgrade("Cannon", "Accuracy", 200, 1.2f, "Cannon Accuracy", "Increases the accuracy of the cannon", 5);
        addUpgrade("Cannon", "Damage", 350, 1.4f, "Cannon Damage", "Increases the ammount of damage dealth with each shot");
        addUpgrade("Cannon", "FireRate", 475, 1.7f, "Cannon Firerate", "Increases the ammount of bullets shot by the Cannon" , 10);
    }


    ///// <summary>
    ///// Addes a new upgrade
    ///// </summary>
    ///// <param name="UpgradeFunc">The function that does the actual upgrading</param>
    ///// <param name="StartCost">The starting cost of the upgrade</param>
    ///// <param name="Mult">set to zero if only buy one. How steep the increase curve is</param>
    ///// <param name="StartLevel">The level the upgrade has when the scene is loaded</param>
    ///// <param name="name"> The name that will be displayed on screen for the upgrade</param>
    ///// <param name="discription">The discription that the upgrade will be given on screen</param>
    //void addUpgrade(Func<int, bool, bool> UpgradeFunc, float StartCost, float Mult, int StartLevel, string name, string discription)
    //{
    //    UpgradeItem e = Instantiate(ParentUpgrade, Vector3.zero, Quaternion.identity) as UpgradeItem;
    //    e.transform.SetParent(ContentParent, false);
    //    e.gameObject.SetActive(true);
    //    e.Name.text = name;
    //    e.Discription.text = discription;

    //    Upgrades.Add(new UpgradeObject(UpgradeFunc, StartCost, Mult, StartLevel, e));
    //}

    /// <summary>
    /// Addes a new upgrade using the new system. It gets a value by its name and edit's it that way.
    /// This way it does not require a function for every upgrade making it much more effecient.
    /// </summary>
    /// <param name="system">The name of the struct Leave empty when no struct applies</param>
    /// <param name="refrence">The name of the varible as it's named in UpgradeData.cs</param>
    /// <param name="StartCost">The intal cost of the upgrade</param>
    /// <param name="Mult">set to zero it value is a bool. </param>
    /// <param name="name"> The name that will be displayed on screen for the upgrade</param>
    /// <param name="discription">The discription that the upgrade will be given on screen</param>
    /// <param name="MaxLevel">The maxium level of the upgrade. Leave it to default when wanting to disable</param>
    void addUpgrade(string system, string refrence, float StartCost, float Mult, string name, string discription, int MaxLevel = -1)
    {
        UpgradeItem e = Instantiate(ParentUpgrade, Vector3.zero, Quaternion.identity) as UpgradeItem;
        e.transform.SetParent(ContentParent, false);
        e.gameObject.SetActive(true);
        e.Name.text = name;
        e.Discription.text = discription;

        UpgradeObject u = new UpgradeObject(system, refrence, StartCost, MaxLevel, Mult, e);
        currentCatagory.addNew(u);
        Upgrades.Add(u);
    }

    void addCatagory(string name, string discription)
    {
        UpgradeCatagory u = Instantiate(parentCatagory);
        u.gameObject.SetActive(true);
        u.transform.SetParent(ContentParent, false);

        CatagoryObject o = new CatagoryObject(u, name, discription);

        currentCatagory = o;
        o.ToggleAll();
        cats.Add(o);
    }
    #endregion

    [System.Serializable]
    public class UpgradeObject
    {
        UpgradeItem visual;

        Func<int, bool,bool> UpgradeFunc;

        float StartCost, Mult;

        int Level = 0, MaxLevel =-1;
        string refrence;
        string system;

        public bool enabled { set { visual.gameObject.SetActive(value); } }

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

        public UpgradeObject (string system, string refrence, float StartCost,int MaxLevel ,float Mult, UpgradeItem visual)
        {
            if (system == "")
            {
                var temp = upgradeData.GetType().GetField(refrence).GetValue(upgradeData);

                if (Mult > 0)
                {
                    Level = (int)temp;
                }
                else
                {
                    bool b = (bool)temp;
                    Level = b.toInt();
                }
            }
            else
            {
                var systemTemp = upgradeData.GetType().GetField(system).GetValue(upgradeData);
                if (systemTemp == null)
                    Debug.LogError("No such value as " + system + " in UpgradeData.cs");

                var temp = systemTemp.GetType().GetField(refrence).GetValue(systemTemp);

                if (Mult > 0)
                {
                    Level = (int)temp;
                }
                else
                {
                    bool b = (bool)temp;
                    Level = b.toInt();
                }
            }
            
            
            this.StartCost = StartCost;
            this.Mult = Mult;
            this.refrence = refrence;
            this.visual = visual;
            this.system = system;
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

            if ((UpgradeFunc != null && UpgradeFunc(GetPrice(), pay)) || canBuyUpgrade(pay))
            {
                cb.pressedColor = Color.green;
                cb.highlightedColor = Color.green + Color.gray;
            }
            else
            {
                cb.pressedColor = Color.red;
                cb.highlightedColor = Color.red + Color.gray;
            }
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

        /// <summary>
        /// Uses only the refrence string
        /// </summary>
        /// <param name="substract">just a price check?</param>
        /// <returns></returns>
        bool canBuyCheck(bool substract)
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
        /// <summary>
        /// Uses both the System and Refrence string
        /// </summary>
        /// <param name="substact">just a price check?</param>
        /// <returns></returns>
        bool canBuyCheck2(bool substract)
        {
            if (upgradeData.UpgradeCurrency >= GetPrice() && (MaxLevel < 0 || MaxLevel > 0 && Level < MaxLevel) && (Mult > 0 || Mult <= 0 && Level <= 0))
            {
                if (substract)
                {
                    var sys = upgradeData.GetType().GetField(system).GetValue(upgradeData);
                    var dat = sys.GetType().GetField(refrence).GetValue(sys);

                    if (Mult > 0)
                    {

                        if (MaxLevel > 0 && Level >= MaxLevel)
                            return false;

                        upgradeData.UpgradeCurrency -= GetPrice();
                        int f;

                        f = (int)dat;
                        Level++;
                        f = Level;

                        sys.GetType().GetField(refrence).SetValue(upgradeData, f);
                        upgradeData.GetType().GetField(system).SetValue(upgradeData, sys);
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

                        sys.GetType().GetField(refrence).SetValue(upgradeData, f);
                        upgradeData.GetType().GetField(system).SetValue(upgradeData, sys);
                    }

                }
                return true;
            }
            return false;
        }

        bool canBuyUpgrade(bool substract)
        {
            if (system == "")
                return canBuyCheck(substract);
            else
                return canBuyCheck2(substract);
        }

        public void Move(Vector2 newpos)
        {
            ((RectTransform)visual.transform).anchoredPosition = newpos;
        }
    }

    [System.Serializable]
    public class CatagoryObject
    {
        UpgradeCatagory visual;
        List<UpgradeObject> subObjs = new List<UpgradeObject>();
        bool active = true;

        public CatagoryObject (UpgradeCatagory visual, string name, string discription)
        {
            this.visual = visual;
            visual.SetText(name, discription);

            visual.ButtonObj.onClick.AddListener(delegate
            {
                ToggleAll();
            });
        }

        public void addNew(UpgradeObject o)
        {
            o.enabled = active;
            subObjs.Add(o);
        }

        public void ToggleAll()
        {
            active = !active;
            Vector3 newpos = ((RectTransform)visual.transform).anchoredPosition;
            foreach (UpgradeObject o in subObjs)
            {
                o.enabled = active;
                o.Move(newpos);
            }
        }
    }
}


