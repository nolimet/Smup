using System;
using System.Collections.Generic;
using Menus.MenuParts;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Util;
using Util.Saving;

namespace UpgradeSystem
{
    public class UpgradeMenu : MonoBehaviour
    {
        [SerializeField] private UpgradeData upgrades;

        [FormerlySerializedAs("UpgradeCurrency")] [SerializeField] private Text upgradeCurrency;
        [FormerlySerializedAs("ParentUpgrade")] [SerializeField] private UpgradeItem parentUpgrade;
        [FormerlySerializedAs("parentCatagory")] [SerializeField] private UpgradeCatagory parentCategory;
        [FormerlySerializedAs("ContentParent")] [SerializeField] private Transform contentParent;

        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<UpgradeObject> _upgrades = new(); //used to keep some data alive and in memory

        // ReSharper disable once CollectionNeverQueried.Local
        private readonly List<CategoryObject> _cats = new(); //used to keep some data alive and in memory

        private CategoryObject _currentCategory;

        private static UpgradeMenu _instance;
        private static UpgradeData Data => _instance.upgrades;

        private void Awake()
        {
            if (!_instance)
            {
                _instance = this;
            }

            Serialization.Load("upgrade", Serialization.FileTypes.Binary, out upgrades);

            if (upgrades == null)
            {
                upgrades = new UpgradeData();
                Serialization.Save("upgrade", Serialization.FileTypes.Binary, upgrades);
            }

            parentUpgrade.gameObject.SetActive(false);
            parentCategory.gameObject.SetActive(false);
            InitUpgradeFunctions();
        }

        private void Update()
        {
            if (!_instance)
            {
                _instance = this;
            }

            upgradeCurrency.text = $"{upgrades.upgradeCurrency}$";
        }

        private void OnDestroy()
        {
            Serialization.Save("upgrade", Serialization.FileTypes.Binary, upgrades);
        }

        public void OnDisable()
        {
            Serialization.Save("upgrade", Serialization.FileTypes.Binary, upgrades);
        }

        /// <summary>
        /// A init function for all upgrades
        /// </summary>
        private void InitUpgradeFunctions()
        {
            //scrapCollection
            AddCategory("Scrap Collection", "Improve your scrap collection ability");
            AddUpgrade(string.Empty, nameof(UpgradeData.scrapConversionRate), 900, 1.3f, "Scrap sell rate", "Improves for how much you sell each piece of scrap");
            AddUpgrade(string.Empty, nameof(UpgradeData.scrapCollectionRange), 650, 1.3f, "Scrap Collection Range", "Makes the collection range bigger", 10);
            AddUpgrade(string.Empty, nameof(UpgradeData.scrapCollectionSpeed), 650, 1.5f, "Scrap Collection Speed", "Improves how fast the scrap moves towards you", 5);

            //Hull
            AddCategory("Ship Upgrades", "Upgrades that make your ship strong, faster and generally better");
            AddUpgrade(string.Empty, nameof(UpgradeData.hullUpgradeLevel), 50, 2f, "Hull Upgrade", "Upgrading the hull allows the ship to take more hits", 10);
            AddUpgrade(string.Empty, nameof(UpgradeData.armorUpgradeLevel), 100, 1.4f, "Armor Upgrade", "Upgrades the amount of armor the ship has");

            //Shield
            AddUpgrade(string.Empty, nameof(UpgradeData.unlockedShield), 1200, 0, "Unlock Shield", "Unlocks the shield");
            AddUpgrade(string.Empty, nameof(UpgradeData.shieldGeneratorLevel), 1300, 1.2f, "Shield Regeneration Speed", "Increases the regeneration speed of the shield");
            AddUpgrade(string.Empty, nameof(UpgradeData.shieldCapacitorLevel), 1800, 1.2f, "Shield capacity upgrade", "Increases the capacity of the shield");

            //shotgun
            AddCategory("Shotgun Upgrades", " Upgrades for your shotgun");
            AddUpgrade(nameof(UpgradeData.shotgun), nameof(CommonWeaponUpgrade.unlocked), 750, 0, "Unlocks Shotgun", "Unlocks the Shotgun");
            AddUpgrade(nameof(UpgradeData.shotgun), nameof(CommonWeaponUpgrade.accuracy), 1425, 1.8f, "Increase Accuracy", "Decreases the spread of the shotgun", 10);
            AddUpgrade(nameof(UpgradeData.shotgun), nameof(CommonWeaponUpgrade.damage), 1250, 1.3f, "Damage per fragment", "Increases the damage each fragment does");
            AddUpgrade(nameof(UpgradeData.shotgun), nameof(CommonWeaponUpgrade.fireRate), 750, 1.3f, "Fragments per Shotgun shot", "Increases the number of fragments shot out per shot", 6);

            //machineGun
            AddCategory("Machine-gun Upgrades", "Upgrades for your machine-gun");
            AddUpgrade(nameof(UpgradeData.miniGun), nameof(CommonWeaponUpgrade.unlocked), 700, 0, "Unlock Machine-gun", "Unlocks the Machine-gun");
            AddUpgrade(nameof(UpgradeData.miniGun), nameof(CommonWeaponUpgrade.accuracy), 3000, 1.3f, "Increase Accuracy", "Increases Accuracy of the Machine-gun", 10);
            AddUpgrade(nameof(UpgradeData.miniGun), nameof(CommonWeaponUpgrade.damage), 4000, 1.1f, "Increase Damage", "Increases the damage each bullet of the machine gun does");
            AddUpgrade(nameof(UpgradeData.miniGun), nameof(CommonWeaponUpgrade.fireRate), 2500, 1.2f, "Increase Fire-rate", "Increases number of bullets shot out by the machine gun every second", 20);

            //Cannon
            AddCategory("Cannon Upgrades", "Upgrades for your main Cannon");
            AddUpgrade(nameof(UpgradeData.cannon), nameof(CommonWeaponUpgrade.accuracy), 200, 1.2f, "Cannon Accuracy", "Increases the accuracy of the cannon", 5);
            AddUpgrade(nameof(UpgradeData.cannon), nameof(CommonWeaponUpgrade.damage), 350, 1.4f, "Cannon Damage", "Increases the amount of damage dealt with each shot");
            AddUpgrade(nameof(UpgradeData.cannon), nameof(CommonWeaponUpgrade.fireRate), 475, 1.7f, "Cannon Fire-rate", "Increases the amount of bullets shot by the Cannon", 20);

            //Granade
            AddCategory("Grenade Upgrades", "Upgrades for a Explosive helper!");
            AddUpgrade(nameof(UpgradeData.grenade), nameof(CommonWeaponUpgrade.unlocked), 5000, 0, "Unlock Grenade", "Unlocks the Grenade");
            AddUpgrade(nameof(UpgradeData.grenade), nameof(CommonWeaponUpgrade.damage), 2000, 1.3f, " Increased Damage", "Increases the damage each piece of shrapnel does");
            AddUpgrade(nameof(UpgradeData.grenade), nameof(CommonWeaponUpgrade.fragments), 1500, 1.7f, "More Shrapnel!", "Increases the number of pieces the grenade explodes into", 30);
            AddUpgrade(nameof(UpgradeData.grenade), nameof(CommonWeaponUpgrade.fireRate), 4000, 2f, "Increased Fire rate", "Increases the rate of firing the grenade", 20);
        }

        /// <summary>
        /// Addes a new upgrade using the new system. It gets a value by its name and edit's it that way.
        /// This way it does not require a function for every upgrade making it much more effecient.
        /// </summary>
        /// <param name="groupName">The name of the struct Leave empty when no struct applies</param>
        /// <param name="fieldName">The name of the varible as it's named in UpgradeData.cs</param>
        /// <param name="startCost">The intal cost of the upgrade</param>
        /// <param name="mult">set to zero it value is a bool. </param>
        /// <param name="displayName"> The name that will be displayed on screen for the upgrade</param>
        /// <param name="description">The discription that the upgrade will be given on screen</param>
        /// <param name="maxLevel">The maxium level of the upgrade. Leave it to default when wanting to disable</param>
        private void AddUpgrade(string groupName, string fieldName, float startCost, float mult, string displayName, string description, int maxLevel = -1)
        {
            var element = Instantiate(parentUpgrade, contentParent, false);
            element.gameObject.SetActive(true);
            element.DisplayName = displayName;
            element.Description = description;

            var upgradeObject = new UpgradeObject(groupName, fieldName, startCost, maxLevel, mult, element);
            _currentCategory.AddNew(upgradeObject);
            _upgrades.Add(upgradeObject);
        }

        private void AddCategory(string displayName, string discription)
        {
            var upgradeCategory = Instantiate(parentCategory, contentParent, false);
            upgradeCategory.gameObject.SetActive(true);

            var newcategoryObject = new CategoryObject(upgradeCategory, displayName, discription);

            _currentCategory = newcategoryObject;
            newcategoryObject.ToggleAll();
            _cats.Add(newcategoryObject);
        }

        [Serializable]
        public class UpgradeObject
        {
            private readonly UpgradeItem _visual;

            private Func<int, bool, bool> _upgradeFunc;

            private readonly float _startCost, _mult;

            private int _level;
            private readonly int _maxLevel;
            private readonly string _fieldName, _groupName;

            public bool Enabled
            {
                set => _visual.gameObject.SetActive(value);
            }

            /// <summary>
            /// Adds a new upgrade object
            /// </summary>
            /// <param name="groupName">Sub location of the variable</param>
            /// <param name="fieldName">name of the upgrade variable</param>
            /// <param name="startCost">The starting cost of the Upgrade</param>
            /// <param name="maxLevel">The maximum level set to zero for unlimted upgrades</param>
            /// <param name="mult"> by how much the upgrade goes up each time</param>
            /// <param name="visual">The object that will be shown on screen</param>
            public UpgradeObject(string groupName, string fieldName, float startCost, int maxLevel, float mult, UpgradeItem visual)
            {
                object groupValue = Data;
                if (!string.IsNullOrEmpty(groupName))
                {
                    groupValue = Data.GetType().GetField(groupName).GetValue(Data);
                    Debug.Assert(groupValue != null, $"The group {groupName} was not found");
                    return;
                }

                var fieldValue = groupValue.GetType().GetField(fieldName).GetValue(groupValue);
                if (fieldValue == null)
                {
                    throw new NullReferenceException($"failed to find {fieldName} for {groupName}");
                }

                if (!visual)
                {
                    throw new NullReferenceException($"Missing visual element. This should be set! {groupName}/{fieldName}");
                }

                if (mult > 0)
                {
                    _level = (int) fieldValue;
                }
                else
                {
                    var b = (bool) fieldValue;
                    _level = b.ToInt();
                }

                _startCost = startCost;
                _mult = mult;
                _fieldName = fieldName;
                _visual = visual;
                _groupName = groupName;
                _maxLevel = maxLevel;

                _visual.Price = GetPrice().ToString();
                _visual.BuyButton.onClick.AddListener(Upgrade);

                UpdateColour();
                UpdateLevel();
            }

            public void Upgrade()
            {
                Buy();
                UpdateColour();
                _visual.Price = GetPrice().ToString();
                UpdateLevel();
            }

            private int GetPrice()
            {
                if (_mult > 0)
                {
                    return (int) (_startCost * Mathf.Pow(_mult, _level));
                }
                return (int) _startCost;
            }

            private bool CanAfford() => Data.upgradeCurrency >= GetPrice();

            private void UpdateColour()
            {
                var colorBlock = _visual.BuyButton.colors;

                if (CanAfford() && !UpgradeMaxed())
                {
                    colorBlock.pressedColor = Color.green;
                    colorBlock.highlightedColor = Color.green + Color.gray;
                }
                else
                {
                    colorBlock.pressedColor = Color.red;
                    colorBlock.highlightedColor = Color.red + Color.gray;
                }

                _visual.BuyButton.colors = colorBlock;
            }

            private void UpdateLevel()
            {
                if (_mult > 0)
                {
                    _visual.CurrentLevel = $"Level \n{_level}";
                    if (_maxLevel > 0)
                    {
                        _visual.CurrentLevel += $"/{_maxLevel}";
                    }
                }
                else
                {
                    _visual.CurrentLevel = "Unlocked \n";
                    if (_level == 0)
                    {
                        _visual.CurrentLevel += "NO";
                    }
                    else
                    {
                        _visual.CurrentLevel += "YES";
                    }
                }
            }

            /// <summary>
            /// Has the upgrades last leven been bought or just unlocked.
            /// </summary>
            /// <returns>True if it was maxed out, false if it was not</returns>
            private bool UpgradeMaxed()
            {
                var groupValue = !string.IsNullOrEmpty(_groupName) ? Data.GetType().GetField(_groupName)?.GetValue(Data) ?? Data : Data;
                var fieldValue = groupValue.GetType().GetField(_fieldName).GetValue(groupValue);

                return fieldValue switch
                {
                    int level when level >= _maxLevel || level == -1 => true,
                    bool when true => true,
                    _ => false
                };
            }

            private bool Buy()
            {
                if (!CanAfford() || UpgradeMaxed())
                {
                    return false;
                }

                var groupValue = Data.GetType().GetField(_groupName)?.GetValue(Data) ?? Data;
                var fieldValue = groupValue.GetType().GetField(_fieldName).GetValue(groupValue);

                switch (fieldValue)
                {
                    case int level:
                        Data.upgradeCurrency -= GetPrice();
                        _level = ++level;
                        groupValue.GetType().GetField(_fieldName).SetValue(Data, level);
                        break;

                    case bool:
                        Data.upgradeCurrency -= GetPrice();

                        _level = 1;
                        groupValue.GetType().GetField(_fieldName).SetValue(Data, true);
                        break;
                }

                return true;
            }

            public void Move(Vector2 newpos)
            {
                ((RectTransform) _visual.transform).anchoredPosition = newpos;
            }
        }

        [Serializable]
        public class CategoryObject
        {
            private UpgradeCatagory _visual;
            private List<UpgradeObject> _subObjs = new();
            private bool _active = true;

            public CategoryObject(UpgradeCatagory visual, string name, string discription)
            {
                _visual = visual;
                visual.SetText(name, discription);

                visual.ExpandButton.onClick.AddListener(ToggleAll);
            }

            public void AddNew(UpgradeObject o)
            {
                o.Enabled = _active;
                _subObjs.Add(o);
            }

            public void ToggleAll()
            {
                _active = !_active;
                Vector3 anchoredPosition = ((RectTransform) _visual.transform).anchoredPosition;
                foreach (var upgradeObject in _subObjs)
                {
                    upgradeObject.Enabled = _active;
                    upgradeObject.Move(anchoredPosition);
                }
            }
        }
    }
}
