using System.Reflection;
using Managers;
using UnityEngine.UIElements;
using UpgradeSystem.Attributes;

namespace UpgradeSystem.UI.Elements
{
	[UxmlElement]
	public partial class UpgradeContainer : VisualElement
	{
		public const string ussClassName = "upgrade-container";

		public UpgradeContainer()
		{
			AddToClassList(ussClassName);
			GenerateElements();
		}

		private void GenerateElements()
		{
			var dat = SaveDataManager.Upgrades;
			CategoryElement currentCategoryElement = null;
			foreach (var fieldInfo in UpgradeData.GetFields())
			{
				var categoryAttribute = fieldInfo.GetCustomAttribute<CategoryAttribute>();
				var elementAttribute = fieldInfo.GetCustomAttribute<ElementAttribute>();

				if (categoryAttribute is not null)
				{
					currentCategoryElement = new CategoryElement(categoryAttribute);
					Add(currentCategoryElement);
				}

				var val = fieldInfo.GetValue(dat);
				switch (val)
				{
					case CommonWeaponUpgrade weaponUpgrade when categoryAttribute is not null:
						Add(new WeaponUpgradeGroup(categoryAttribute, weaponUpgrade));
						break;
					case Upgradable upgradable when elementAttribute is not null && currentCategoryElement is not null:
						currentCategoryElement.Add(new UpgradableElement(elementAttribute, upgradable));
						break;
				}
			}
		}
	}
}
