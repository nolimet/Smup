using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Smup.Util.StateMachine;
using Smup.Util.StateMachine.Interfaces;
using Smup.World.Waves.Interfaces;
using UnityEngine;

namespace Smup.World.Level.States
{
    [Serializable]
    public class WaveState : IState, IStateMachine
    {
        public event Action Stopped;
        public StateMachineRuntime Runtime { get; private set; }
        public StateMachineRuntime CurrentStateMachine { get; set; }

        [TypeFilter(nameof(TypeFilter))]
        [SerializeReference] private List<IState> sequence = new();

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

            Runtime = new StateMachineRuntime(sequence, true);
            Runtime.EndStateEntered += CurrentStateMachine.ToNextState;
            Runtime.ToFirstState();
        }

        public void StopStateMachine()
        {
            if (Runtime.CurrentState is not EndState)
            {
                Runtime.EndStateEntered -= CurrentStateMachine.ToNextState;
                Runtime?.ToEndState();
            }

            Runtime = null;
        }
    }
}
