using Smup.UI.MainMenu.Elements;
using Smup.Util.StateMachine;
using Smup.Util.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.States
{
    public class MainMenuViewState : IState
    {
        private readonly MainMenuView _mainMenuView;

        public MainMenuViewState(MainMenuView mainMenuView) => _mainMenuView = mainMenuView;

        public StateMachineRuntime CurrentStateMachine { get; set; }

        public void OnEnter()
        {
            _mainMenuView.style.display = DisplayStyle.Flex;

            _mainMenuView.StartClicked += OnStartClicked;
            _mainMenuView.UpgradesClicked += OnUpgradeOpenClicked;
            _mainMenuView.ExitClicked += OnExitClicked;
        }

        private void OnStartClicked()
        {
            CurrentStateMachine.ToState(s => s is StartGameState);
        }

        private void OnUpgradeOpenClicked()
        {
            CurrentStateMachine.ToState(s => s is UpgradeScreenState);
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }

        public void OnExit()
        {
            _mainMenuView.StartClicked -= OnStartClicked;
            _mainMenuView.UpgradesClicked -= OnUpgradeOpenClicked;
            _mainMenuView.ExitClicked -= OnExitClicked;

            _mainMenuView.style.display = DisplayStyle.None;
        }
    }
}
