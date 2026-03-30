using Smup.UI.MainMenu.Elements;
using Smup.UI.MainMenu.Upgrade.Elements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu
{
    [UxmlElement]
    public partial class MainMenuContainer : VisualElement
    {
        private readonly MainMenuView _mainMenuView;
        private readonly UpgradeContainer _upgradeContainer;

        public MainMenuContainer()
        {
            Add(_upgradeContainer = new UpgradeContainer());
            Add(_mainMenuView = new MainMenuView());

            _mainMenuView.StartClicked += OnStartClicked;
            _mainMenuView.UpgradesClicked += OnUpgradeOpenClicked;
            _mainMenuView.ExitClicked += OnExitClicked;

            _upgradeContainer.CloseClicked += OnCloseUpgradeClicked;

            _upgradeContainer.style.display = DisplayStyle.Flex;
            _mainMenuView.style.display = DisplayStyle.None;
        }

        private void OnStartClicked()
        {
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        private void OnCloseUpgradeClicked()
        {
            _upgradeContainer.style.display = DisplayStyle.None;
            _mainMenuView.style.display = DisplayStyle.Flex;
        }

        private void OnUpgradeOpenClicked()
        {
            _mainMenuView.style.display = DisplayStyle.None;
            _upgradeContainer.style.display = DisplayStyle.Flex;
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}
