using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Util.StateMachine;
using Util.StateMachine.Interfaces;
using World.Waves.Interfaces;

namespace World.Level.States
{
    [Serializable]
    public class WaveState : IState, IStateMachine
    {
        public event Action Stopped;
        public StateMachineRuntime Runtime { get; private set; }
        public StateMachineRuntime StateMachine { get; set; }

        [TypeFilter(nameof(TypeFilter))]
        [SerializeReference] private List<IState> sequence;

        public IEnumerable<Type> TypeFilter()
        {
            return typeof(WaveState).Assembly.GetTypes().Where(x => x.IsSerializable && x.GetInterfaces().Contains(typeof(ISequenceElement)));
        }

        public void OnEnter()
        {
            StartStateMachine();
        }

        public void OnExit()
        {
            StopStateMachine();
        }

        public void StartStateMachine()
        {
            if (Runtime != null) Runtime.EndStateEntered -= StopStateMachine;
            Runtime?.ToEndState();

            Runtime = new StateMachineRuntime(sequence);
            Runtime.EndStateEntered += StopStateMachine;
            Runtime.ToFirstState();
        }

        public void StopStateMachine()
        {
            if (Runtime.CurrentState is not EndState)
                Runtime?.ToEndState();
            Runtime = null;
        }
    }
}
