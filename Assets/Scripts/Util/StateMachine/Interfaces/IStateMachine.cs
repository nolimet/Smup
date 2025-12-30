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

        /// <summary>
        /// Initiates the execution of the state machine by setting its runtime and
        /// transitioning to the first state. This method configures the
        /// <see cref="StateMachineRuntime"/> associated with the state machine
        /// and starts state transitions.
        /// </summary>
        /// <remarks>
        /// When the final state of the state machine is entered, the
        /// <see cref="Stopped"/> event is invoked to signal completion.
        /// The runtime must be properly initialized before invoking this method.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the state machine's runtime fails to initialize or is already running.
        /// </exception>
        public void StartStateMachine();

        /// <summary>
        /// Stops the execution of the state machine and transitions it to the final state.
        /// This method cleans up the associated <see cref="StateMachineRuntime"/> and
        /// ensures the state machine is no longer active.
        /// </summary>
        /// <remarks>
        /// After invoking this method, the state machine's runtime will be nullified,
        /// indicating that it is no longer operational. The transition to the end state
        /// ensures any necessary cleanup steps or final state logic are executed.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the state machine is already stopped or no runtime exists to manage the transition.
        /// </exception>
        public void StopStateMachine();
    }
}
