using Util.StateMachine.Interfaces;

namespace Util.StateMachine
{
    /// <summary>
    /// Represents the final state in a state machine.
    /// This state signifies the termination or conclusion of the state machine's processing sequence.
    /// </summary>
    public sealed class EndState : IState
    {
        public StateMachineRuntime StateMachineRuntime { get; set; }

        public void OnEnter() { }
        public void OnExit() { }
    }
}
