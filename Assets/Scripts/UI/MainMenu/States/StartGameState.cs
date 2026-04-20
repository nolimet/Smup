using Smup.Util.StateMachine;
using Smup.Util.StateMachine.Interfaces;
using UnityEngine.SceneManagement;

namespace Smup.UI.MainMenu.States
{
    public class StartGameState : IState
    {
        public StateMachineRuntime CurrentStateMachine { get; set; }

        public void OnEnter()
        {
            SceneManager.LoadScene("Scenes/Game", LoadSceneMode.Single);
        }

        public void OnExit() { }
    }
}
