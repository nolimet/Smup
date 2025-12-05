using System.Reflection;
using UpgradeSystem.Attributes;

namespace UpgradeSystem.UI.Elements
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
