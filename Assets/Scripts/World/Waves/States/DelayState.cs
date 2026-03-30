using System;
using Cysharp.Threading.Tasks;
using Smup.Util.StateMachine;
using Smup.World.Waves.Interfaces;
using UnityEngine;

namespace Smup.World.Waves.States
{
    [Serializable]
    public class DelayState : ISequenceElement
    {
        public StateMachineRuntime CurrentStateMachine { get; set; }

        [SerializeField] [Min(0)] private int delayInMs;

        public void OnEnter()
        {
            UniTask.Delay(delayInMs).ContinueWith(() => CurrentStateMachine.ToNextState()).Forget();
        }

        public void OnExit() { }
    }
}
