using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Util.StateMachine;
using World.Waves.Interfaces;

namespace World.Waves.States
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
