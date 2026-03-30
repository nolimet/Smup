using System.Reflection;
using Smup.UpgradeSystem;
using Smup.UpgradeSystem.Attributes;

namespace Smup.UI.MainMenu.Upgrade.Elements
{
    internal class WeaponUpgradeGroup : CategoryElement
    {
        public WeaponUpgradeGroup(CategoryAttribute categoryAttr, CommonWeaponUpgrade group) : base(categoryAttr)
        {
            foreach (var field in CommonWeaponUpgrade.GetFields())
            {
                var val = field.GetValue(group);
                var elementAttr = field.GetCustomAttribute<ElementAttribute>();
                if (val is Upgradable upgradable) Add(new UpgradableElement(elementAttr, upgradable));
            }
        }
    }
}
