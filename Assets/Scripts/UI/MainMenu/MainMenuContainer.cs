using System.Collections.Generic;
using Smup.UI.MainMenu.Elements;
using Smup.UI.MainMenu.States;
using Smup.UI.MainMenu.Upgrade.Elements;
using Smup.Util.StateMachine;
using Smup.Util.StateMachine.Interfaces;
using UnityEngine;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu
{
    [UxmlElement]
    public partial class MainMenuContainer : VisualElement
    {
        private readonly MainMenuView _mainMenuView;
        private readonly UpgradeContainer _upgradeContainer;
        public StateMachineRuntime Runtime { get; private set; }

        public MainMenuContainer()
        {
            Add(_upgradeContainer = new UpgradeContainer());
            Add(_mainMenuView = new MainMenuView());

            if (Application.isPlaying)
            {
                RegisterCallbackOnce<AttachToPanelEvent>(_ => StartStateMachine());
                RegisterCallbackOnce<DetachFromPanelEvent>(_ => StopStateMachine());
            }
        }

        public void StartStateMachine()
        {
            Runtime = new StateMachineRuntime(new List<IState>
            {
                new SetupState(_mainMenuView, _upgradeContainer),
                new MainMenuViewState(_mainMenuView),
                new UpgradeScreenState(_upgradeContainer),
                new StartGameState()
            }, true);
            Runtime.ToFirstState();
        }

        public void StopStateMachine()
        {
            Runtime.ToEndState();
        }
    }
}
