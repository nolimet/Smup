namespace Util.StateMachine.Interfaces
{
    public interface IState
    {
        /// <summary>
        /// Represents the state machine controller that manages the transitions between different states.
        /// </summary>
        /// <remarks>
        /// The StateMachine property provides access to the underlying runtime mechanism used
        /// for controlling state transitions. It allows states to interact with the state machine
        /// runtime, triggering transitions or accessing the current state. Implementations of IState
        /// use this property to coordinate their behavior within the state machine.
        /// </remarks>
        public StateMachineRuntime StateMachineRuntime { get; set; }

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
