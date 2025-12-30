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
        public StateMachineRuntime StateMachine { get; set; }

        public void OnEnter()
        {
            UniTask.WaitUntil(() => EnemyPool.Instance.ActiveItems == 0).ContinueWith(StateMachine.ToNextState).Forget();
        }

        public void OnExit() { }
    }
}
