using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using Util.StateMachine;
using Util.StateMachine.Interfaces;
using World.Level.States;

namespace World.Level
{
    public class LevelStateMachine : MonoBehaviour, IStateMachine
    {
        public event Action Stopped;
        public StateMachineRuntime Runtime { get; private set; }

        [SerializeField] private bool autoStart;

        [TypeFilter(nameof(TypeFilter))]
        [DisableInPlayMode]
        [SerializeReference] private List<IState> states = new();

        public IEnumerable<Type> TypeFilter()
        {
            return new[] { typeof(WaveState) };
        }

        private void Start()
        {
            if (autoStart) StartStateMachine();
        }

        public void StartStateMachine()
        {
            Runtime = new StateMachineRuntime(states, true);
            Runtime.EndStateEntered += () => Stopped?.Invoke();
            Runtime.ToFirstState();
        }

        public void StopStateMachine()
        {
            Runtime?.ToEndState();
            Runtime = null;
        }
    }
}
