using Util.StateMachine.Interfaces;

namespace Util.StateMachine
{
    public sealed class EndState : IState
    {
        public IStateMachine StateMachine { get; set; }
        public void OnEnter() { }
        public void OnExit() { }
    }
}
