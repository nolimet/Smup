using System;

namespace Util.StateMachine.Interfaces
{
    public interface IStateMachine
    {
        /// <summary>
        /// Event raised when the state machine execution has been stopped.
        /// </summary>
        /// <remarks>
        /// This event is typically triggered when the state machine halts its processing,
        /// either due to reaching a terminal state or an explicit stop command by the user.
        /// </remarks>
        public event Action Stopped;

        /// <summary>
        /// Provides access to the runtime instance of the state machine during its operation.
        /// </summary>
        /// <remarks>
        /// The runtime instance enables interaction with the state machine's execution context,
        /// including methods for transitioning between states and monitoring state changes.
        /// </remarks>
        public StateMachineRuntime Runtime { get; }
    }
}
