using Smup.UI.MainMenu.Upgrade.Elements;
using Smup.Util.StateMachine;
using Smup.Util.StateMachine.Interfaces;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.States
{
    public class UpgradeScreenState : IState
    {
        private readonly UpgradeContainer _upgradeContainer;
        public StateMachineRuntime CurrentStateMachine { get; set; }

        public UpgradeScreenState(UpgradeContainer upgradeContainer) => _upgradeContainer = upgradeContainer;

        public void OnEnter()
        {
            _upgradeContainer.style.display = DisplayStyle.Flex;
            _upgradeContainer.CloseClicked += OnCloseUpgradeClicked;
        }

        private void OnCloseUpgradeClicked()
        {
            CurrentStateMachine.ToState(s => s is MainMenuViewState);
        }

        public void OnExit()
        {
            _upgradeContainer.CloseClicked -= OnCloseUpgradeClicked;
            _upgradeContainer.style.display = DisplayStyle.None;
        }
    }
}
