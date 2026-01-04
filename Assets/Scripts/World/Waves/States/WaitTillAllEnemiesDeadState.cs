using System;
using Cysharp.Threading.Tasks;
using Pools;
using Util.StateMachine;
using World.Waves.Interfaces;

namespace World.Waves.States
{
    [Serializable]
    public class WaitTillAllEnemiesDeadState : ISequenceElement
    {
        public StateMachineRuntime CurrentStateMachine { get; set; }

        public void OnEnter()
        {
            UniTask.WaitUntil(() => EnemyPool.Instance.ActiveItems == 0).ContinueWith(CurrentStateMachine.ToNextState).Forget();
        }

        public void OnExit() { }
    }
}
