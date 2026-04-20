using Smup.UI.MainMenu.Elements;
using Smup.UI.MainMenu.Upgrade.Elements;
using Smup.Util.StateMachine;
using Smup.Util.StateMachine.Interfaces;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.States
{
    public class SetupState : IState
    {
        private readonly MainMenuView _mainMenuView;
        private readonly UpgradeContainer _upgradeContainer;

        public StateMachineRuntime CurrentStateMachine { get; set; }

        public SetupState(MainMenuView mainMenuView, UpgradeContainer upgradeContainer)
        {
            _mainMenuView = mainMenuView;
            _upgradeContainer = upgradeContainer;
        }

        public void OnEnter()
        {
            _upgradeContainer.style.display = DisplayStyle.None;
            _mainMenuView.style.display = DisplayStyle.None;

            CurrentStateMachine.ToNextState();
        }

        public void OnExit() { }
    }
}
