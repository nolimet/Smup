using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Util.StateMachine.Interfaces;

namespace Util.StateMachine
{
    public class StateMachineRuntime
    {
        public delegate void StateChangeDelegate(IState from, IState to);

        public event StateChangeDelegate StateChanged;
        public event Action EndStateEntered;

        private readonly List<IState> _states;
        private int _currentStateIndex;

        public IState CurrentState => _states[_currentStateIndex];

        public StateMachineRuntime([NotNull] List<IState> states)
        {
            _states = states ?? throw new ArgumentNullException(nameof(states));

            if (_states.Count == 0)
                throw new ArgumentException("states list is empty", nameof(states));

            if (_states[^1] is not EndState)
                _states.Add(new EndState());
        }

        public void ToNextState() { }

        public void ToState([NotNull] IState state)
        {
            var newIndex = _states.IndexOf(state);
            ToState(newIndex);
        }

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

        public void ToEndState()
        {
            ToState(_states.Count - 1);
        }
    }
}
