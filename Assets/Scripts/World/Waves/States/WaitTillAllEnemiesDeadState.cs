using System;
using Cysharp.Threading.Tasks;
using Smup.Pools;
using Smup.Util.StateMachine;
using Smup.World.Waves.Interfaces;

namespace Smup.World.Waves.States
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
