using System.Globalization;
using Managers;
using UnityEngine.UIElements;
using UpgradeSystem.Attributes;

namespace UpgradeSystem.UI.Elements
{
    public class UpgradableElement : VisualElement
    {
        public const string ussClassName = "upgrade";

        private const string ussNameElementClassName = ussClassName + "__name";
        private const string ussDescriptionElementClassName = ussClassName + "__description";
        private const string ussCurrentLevelElementClassName = ussClassName + "__current-level";
        private const string ussUpgradeCostClassName = ussClassName + "__cost";
        private const string ussUpgradeButtonClassName = ussClassName + "__button";

        public const string ussLeftContainerClassName = ussClassName + "__left-container";
        public const string ussRightContainerClassName = ussClassName + "__right-container";

        private readonly Label _currentLevelElement;
        private readonly Label _upgradeCostElement;

        private readonly Upgradable _upgradable;

        private UpgradableElement()
        {
            AddToClassList(ussClassName);
        }

        public UpgradableElement(ElementAttribute elementAttr, Upgradable upgradable) : this()
        {
            _upgradable = upgradable;

            var leftContainer = new VisualElement();
            leftContainer.AddToClassList(ussLeftContainerClassName);
            Add(leftContainer);
            {
                var nameElement = new Label(elementAttr.Name);
                nameElement.AddToClassList(ussNameElementClassName);
                leftContainer.Add(nameElement);

                var descriptionElement = new Label(elementAttr.Description);
                descriptionElement.AddToClassList(ussDescriptionElementClassName);
                leftContainer.Add(descriptionElement);
            }

            var rightContainer = new VisualElement();
            rightContainer.AddToClassList(ussRightContainerClassName);
            Add(rightContainer);
            {
                _currentLevelElement = new Label();
                _currentLevelElement.AddToClassList(ussCurrentLevelElementClassName);
                rightContainer.Add(_currentLevelElement);
                UpdateCurrentLevel();

                _upgradeCostElement = new Label();
                _upgradeCostElement.AddToClassList(ussUpgradeCostClassName);
                rightContainer.Add(_upgradeCostElement);
                UpdateCost();

                var upgradeButton = new Button(OnUpgradeClicked) { text = "Upgrade" };
                upgradeButton.AddToClassList(ussUpgradeButtonClassName);
                rightContainer.Add(upgradeButton);
            }
        }

        private void OnUpgradeClicked()
        {
            if (!SaveDataManager.Upgrades.CanAfford(_upgradable.Cost)) return;

            _upgradable.IncrementLevel();
            SaveDataManager.Upgrades.SubtractCurrency(_upgradable.Cost);
            UpdateCurrentLevel();
            UpdateCost();
        }

        private void UpdateCurrentLevel()
        {
            var (current, max) = _upgradable.GetLevels();
            if (max == 1)
                _currentLevelElement.text = current != max ? "Locked" : "Unlocked";
            else
                _currentLevelElement.text = current == max ? $"{max}" : $"{current}/{max}";
        }

        private void UpdateCost()
        {
            var (current, max) = _upgradable.GetLevels();
            _upgradeCostElement.text = current == max ? "MAXED" : _upgradable.Cost.ToString("C", CultureInfo.InvariantCulture);
        }
    }
}
