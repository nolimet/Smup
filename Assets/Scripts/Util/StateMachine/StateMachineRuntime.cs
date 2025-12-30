using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Util.StateMachine.Interfaces;

namespace Util.StateMachine
{
    /// <summary>
    /// Manages the execution and transitions of a finite state machine, tracking the current state
    /// and supporting state transitions through a predefined list of states.
    /// </summary>
    /// <remarks>
    /// This class provides functionality to run a state machine with an ordered list of states.
    /// The final state is automatically set to an <see cref="EndState"/> if not explicitly included.
    /// It supports state transitions, raises events to indicate state changes, and notifies
    /// when the terminal state is entered.
    /// </remarks>
    /// <events>
    /// <list>
    /// <item>
    /// <term><see cref="StateChanged"/></term>
    /// <description>Fired when a state transition occurs, providing the previous and current states.</description>
    /// </item>
    /// <item>
    /// <term><see cref="EndStateEntered"/></term>
    /// <description>Fired when the terminal state is entered.</description>
    /// </item>
    /// </list>
    /// </events>
    /// <example>
    /// This class is initialized with a list of states. Upon initialization, the state machine operates
    /// sequentially or can transition to any state by index or reference.
    /// </example>
    /// <exception cref="ArgumentNullException">
    /// Thrown when the provided list of states is null upon initialization.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// Thrown when the provided list of states is empty upon initialization.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when attempting to transition beyond the last state in the sequence.
    /// </exception>
    public class StateMachineRuntime
    {
        /// <summary>
        /// Represents a delegate that defines the structure for handling state transitions in a state machine.
        /// </summary>
        /// <remarks>
        /// This delegate is used to notify subscribers about state transitions in the state machine.
        /// It provides the previous state and the new state as parameters, enabling handling of specific
        /// logic related to transitions between states.
        /// </remarks>
        /// <param name="from">The state the state-machine is transitioning from.</param>
        /// <param name="to">The state the state-machine is transitioning to.</param>
        public delegate void StateChangeDelegate(IState from, IState to);

        /// <summary>
        /// Occurs when the state machine transitions from one state to another.
        /// </summary>
        /// <remarks>
        /// This event is triggered whenever the state machine changes its active state. The event provides
        /// the previous state and the new state as arguments to the registered event handlers. It allows
        /// subscribers to react to state changes and implement logic tied to specific state transitions.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the state machine attempts to transition to an invalid or undefined state during runtime.
        /// </exception>
        public event StateChangeDelegate StateChanged;

        /// <summary>
        /// Occurs when the state machine enters the terminal state (EndState).
        /// </summary>
        /// <remarks>
        /// This event is triggered precisely once when the state machine transitions to the terminal state
        /// in the predefined sequence of states. Subscribers can use this event to execute finalization
        /// logic or cleanup tasks tied to the conclusion of the state machine's execution.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if an invalid attempt is made to enter the terminal state outside the designated sequence.
        /// </exception>
        public event Action EndStateEntered;

        private readonly List<IState> _states;
        private int _currentStateIndex;

        /// <summary>
        /// Gets the current state of the state machine.
        /// </summary>
        /// <remarks>
        /// This property returns the state object that represents the current active state
        /// of the state machine. The state is determined based on the current state index
        /// within the defined list of states.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the state machine is in an invalid state or the current state index
        /// is out of bounds of the list of states.
        /// </exception>
        public IState CurrentState => _states[_currentStateIndex];

        /// <summary>
        /// Manages the execution and transitions of a finite state machine, tracking the current state and supporting state transitions.
        /// </summary>
        /// <remarks>
        /// This class operates on a predefined sequence of states. A terminal state (of type <see cref="EndState"/>) is automatically added
        /// if not provided in the list of states. It raises events for state changes and entering the terminal state.
        /// </remarks>
        /// <exception cref="ArgumentNullException">Thrown if the list of states is null during initialization.</exception>
        /// <exception cref="ArgumentException">Thrown if the list of states is empty during initialization.</exception>
        public StateMachineRuntime([NotNull] List<IState> states)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));

            if (_states.Count == 0)
                throw new ArgumentException("states list is empty", nameof(states));

            if (_states[^1] is not EndState)
                _states.Add(new EndState());
        }

        /// <summary>
        /// Transitions the state machine to the next state in the sequence.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if attempting to transition beyond the last state in the sequence.</exception>
        public void ToNextState() { }

        /// <summary>
        /// Transitions the state machine to the specified state.
        /// </summary>
        /// <param name="state">The target state to transition to. Must not be null and must exist in the defined list of states.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided state is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided state does not exist in the list of states.</exception>
        public void ToState([NotNull] IState state)
        {
            var newIndex = _states.IndexOf(state);
            ToState(newIndex);
        }

        /// <summary>
        /// Transitions the state machine to the state at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the target state. Must be within the valid range of state indices.</param>
        /// <exception cref="IndexOutOfRangeException">Thrown if the specified index is outside the bounds of the state list.</exception>
        public void ToState(int index)
        {
            if (index < 0 || index >= _states.Count)
                throw new IndexOutOfRangeException($"index {index} is out of range(0-{_states.Count - 1})", new ArgumentOutOfRangeException(nameof(index)));

            if (index == _currentStateIndex) return;

            CurrentState.OnExit();
            var newState = _states[index];
            newState.OnEnter();

            StateChanged?.Invoke(CurrentState, newState);
            _currentStateIndex = index;

            if (CurrentState is EndState)
                EndStateEntered?.Invoke();
        }

        /// <summary>
        /// Transitions the state machine to the end state.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown if the end state is not present at the last position in the list of states.
        /// </exception>
        public void ToEndState()
        {
            ToState(_states.Count - 1);
        }
    }
}
