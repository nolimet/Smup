using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

public class UpgradeMenu : MonoBehaviour
{

    [SerializeField]
    UpgradeData upgrades;

    public Text UpgradeCurrency;

    public UpgradeObject hull, unlockShotgun, unlockMachineGun;
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

    void InitUpgradeFunctions()
    {
        hull.init(UpgradeHullLevel, GetHullUpgradePrice);
        unlockShotgun.init(UnlockShotGun, UnlockShotGunPrice);
        unlockMachineGun.init(UnlockMachineGun, UnlockMachineGunPrice);
    }
    #endregion

    #region public functions
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

    [System.Serializable]
    public class UpgradeObject
    {
        public Text PriceText;
        public Button UpgradeButton;

        [SerializeField]
        Func<bool> UpgradeFunc;
        Func<int> Pricefunc;

        public void init(Func<bool> UpgradeFunc, Func<int> Pricefunc)
        {
            PriceText.text = Pricefunc().ToString();

            this.UpgradeFunc = UpgradeFunc;
            this.Pricefunc = Pricefunc;

            UpgradeButton.onClick.AddListener(delegate { Upgrade(); });
        }

        public void Upgrade()
        {
            ColorBlock cb = UpgradeButton.colors;

            if (UpgradeFunc())
                cb.pressedColor = Color.green;
            else
                cb.pressedColor = Color.red;

            UpgradeButton.colors = cb;
            PriceText.text = Pricefunc().ToString();
        }
    }
}
