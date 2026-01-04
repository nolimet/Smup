using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
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
        private readonly bool _debuggingEnabled;

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
        public StateMachineRuntime([NotNull] List<IState> states, bool debuggingEnabled = false)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));

            if (_states.Count == 0)
                throw new ArgumentException("states list is empty", nameof(states));

            if (_states[^1] is not EndState)
                _states.Add(new EndState());

            _currentStateIndex = -1;
            _debuggingEnabled = debuggingEnabled;
        }

        /// <summary>
        /// Resets the state machine to the first state in the predefined sequence of states.
        /// </summary>
        /// <remarks>
        /// This method triggers a transition to the first state in the state machine and raises
        /// the <see cref="StateChanged"/> event if a transition occurs. It can only transition
        /// to the first state defined in the list of states during initialization.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the state machine contains no states or is in an invalid state.
        /// </exception>
        public void ToFirstState()
        {
            ToState(0);
        }

        /// <summary>
        /// Advances the state machine to the next state in the predefined sequence of states.
        /// </summary>
        /// <remarks>
        /// This method transitions the state machine to the state immediately following the current state
        /// in the internal state list. If there are no more states in the list, it transitions to the terminal state.
        /// </remarks>
        /// <exception cref="InvalidOperationException">
        /// Thrown if called when the state machine is already at the terminal state or the state sequence is improperly configured.
        /// </exception>
        public void ToNextState() => ToState(_currentStateIndex + 1);

        /// <summary>
        /// Transitions the state machine to the specified state.
        /// </summary>
        /// <param name="state">The target state to transition to. Must not be null and must exist in the defined list of states.</param>
        /// <exception cref="ArgumentNullException">Thrown if the provided state is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the provided state does not exist in the list of states.</exception>
        public void ToState([NotNull] IState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

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

            IState currentState = null;
            if (_currentStateIndex >= 0) currentState = _states[_currentStateIndex];

            currentState?.OnExit();
            if (currentState != null) currentState.StateMachineRuntime = null;

            var newState = _states[index];
            newState.StateMachineRuntime = this;
            newState.OnEnter();

            if (_debuggingEnabled) Debug.Log($"Transitioning from ({_currentStateIndex}){currentState?.GetType().Name} to({index}){_states[index].GetType().Name}");

            StateChanged?.Invoke(currentState, newState);
            _currentStateIndex = index;

            if (CurrentState is EndState)
                EndStateEntered?.Invoke();
        }

        /// <summary>
        /// Transitions to a state that matches the provided condition.
        /// The method iterates through all available states and selects the first one that satisfies the given condition.
        /// </summary>
        /// <param name="expression">A predicate function that specifies the condition a state must satisfy to be selected.</param>
        /// <exception cref="ArgumentNullException">Thrown when the expression is null.</exception>
        /// <remarks>
        /// If no state satisfies the provided condition, a warning is logged, and no transition occurs.
        /// </remarks>
        public void ToState(Func<IState, bool> expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression), "Expression cannot be null");

            for (var i = 0; i < _states.Count; i++)
            {
                if (!expression(_states[i])) continue;

                ToState(i);
                return;
            }

            Debug.LogWarning("Failed to find state matching expression");
        }

        /// <summary>
        /// Transitions the state machine to the terminal state, ensuring the current state becomes the end state.
        /// </summary>
        /// <remarks>
        /// This method sets the state machine to the final state in its predefined sequence of states.
        /// If the current state is not already of type <see cref="EndState"/>, it transitions to the last state in the list.
        /// </remarks>
        /// <exception cref="InvalidOperationException">Thrown if the state machine has not been properly initialized or the list of states is empty.</exception>
        public void ToEndState()
        {
            if (CurrentState is not EndState)
                ToState(_states.Count - 1);
            else throw new InvalidOperationException("state machine is already in terminal state");
        }
    }
}
