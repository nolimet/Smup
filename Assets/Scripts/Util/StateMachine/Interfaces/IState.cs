namespace Util.StateMachine.Interfaces
{
    public interface IState
    {
        /// <summary>
        /// Represents a property that provides access to the state machine associated
        /// with the current state.
        /// </summary>
        /// <remarks>
        /// This property allows the current <see cref="IState"/> implementation to interact
        /// with the parent <see cref="IStateMachine"/> that manages the state transitions
        /// and runtime behavior. It enables states to coordinate with the state machine for
        /// various operations such as transitioning to the next state, accessing runtime
        /// data, or invoking specific events.
        /// </remarks>
        public IStateMachine StateMachine { get; set; }

        /// <summary>
        /// Defines the behavior to execute when entering the current state in the state machine.
        /// This method is invoked during a state transition, allowing the new state
        /// to initialize any necessary resources or perform setup operations
        /// before becoming active. Implementations may leave this method empty
        /// if no specific actions are required on entry.
        /// </summary>
        public void OnEnter();

        /// <summary>
        /// Defines the behavior to execute when exiting the current state in the state machine.
        /// This method is invoked during a state transition, allowing the current state
        /// to perform any necessary cleanup or finalization before the transition completes.
        /// Implementations may leave this method empty if no specific actions are required on exit.
        /// </summary>
        public void OnExit();
    }
}
