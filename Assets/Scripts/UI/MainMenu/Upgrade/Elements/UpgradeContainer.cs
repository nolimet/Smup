using System;
using System.Reflection;
using Smup.Managers;
using Smup.UpgradeSystem;
using Smup.UpgradeSystem.Attributes;
using UnityEngine;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.Upgrade.Elements
{
    [UxmlElement]
    public partial class UpgradeContainer : VisualElement
    {
        public event Action CloseClicked;

        public const string ussClassName = "upgrade-container";
        public const string ussCloseButtonClassName = ussClassName + "__close-button";
        public const string ussUpgradesContainerClassName = ussClassName + "__upgrades-container";

        private readonly ScrollView _scrollView;
        public override VisualElement contentContainer => _scrollView;

        public UpgradeContainer()
        {
            AddToClassList(ussClassName);

            var container = new VisualElement();
            container.AddToClassList(ussClassName + "__container");
            hierarchy.Add(container);

            _scrollView = new ScrollView();
            _scrollView.AddToClassList(ussUpgradesContainerClassName);
            container.Add(_scrollView);

            GenerateElements();

            var closeButton = new Button(() => CloseClicked?.Invoke()) { text = "Close" };
            closeButton.AddToClassList(ussCloseButtonClassName);
            container.Add(closeButton);
        }

        private void GenerateElements()
        {
            var dat = SaveDataManager.Upgrades;
            CategoryElement currentCategoryElement = null;
            foreach (var fieldInfo in UpgradeData.GetFields())
            {
                var categoryAttribute = fieldInfo.GetCustomAttribute<CategoryAttribute>();
                var elementAttribute = fieldInfo.GetCustomAttribute<ElementAttribute>();

                if (categoryAttribute is not null && typeof(Upgradable).IsAssignableFrom(fieldInfo.FieldType)) Add(currentCategoryElement = new CategoryElement(categoryAttribute));

                var val = fieldInfo.GetValue(dat);
                switch (val)
                {
                    case CommonWeaponUpgrade weaponUpgrade when categoryAttribute is not null:
                        Add(new WeaponUpgradeGroup(categoryAttribute, weaponUpgrade));
                        break;
                    case Upgradable upgradable when elementAttribute is not null && currentCategoryElement is not null:
                        currentCategoryElement.Add(new UpgradableElement(elementAttribute, upgradable));
                        break;
                    default:
                        Debug.LogWarning($"Failed to place {fieldInfo.Name}");
                        break;
                }
            }
        }
    }
}
