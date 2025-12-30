using System;

namespace Util.StateMachine.Interfaces
{
    public interface IStateMachine
    {
        public event Action Stopped;
        public StateMachineRuntime Runtime { get; }
    }
}
