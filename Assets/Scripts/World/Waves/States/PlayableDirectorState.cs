using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Pools;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using Util.StateMachine;
using World.Waves.Interfaces;
using Object = UnityEngine.Object;

namespace World.Waves.States
{
    [Serializable]
    public class PlayableDirectorState : ISequenceElement
    {
        public StateMachineRuntime CurrentStateMachine { get; set; }

        [AssetsOnly]
        [SerializeField] private PlayableDirector director;

        [SerializeField] private bool waitTillAllEnemiesDead;

        private PlayableDirector _instance;
        private CancellationTokenSource _cts;

        public void OnEnter()
        {
            _cts = new CancellationTokenSource();
            var parms = new InstantiateParameters
            {
                originalImmutable = true,
                parent = null,
                scene = SceneManager.GetActiveScene(),
                worldSpace = true
            };

            Object.InstantiateAsync(director, parms, _cts.Token).completed += OnInstantiated;
        }

        private void OnInstantiated(AsyncOperation obj)
        {
            if (obj is not AsyncInstantiateOperation<PlayableDirector> asyncOpp) return;

            _instance = asyncOpp.Result[0];
            _instance.stopped += OnDirectorStopped;
            _instance.Play();
        }

        private void OnDirectorStopped(PlayableDirector playableDirector)
        {
            if (!waitTillAllEnemiesDead || EnemyPool.Instance.ActiveItems == 0) ToNextState();
            else UniTask.WaitUntil(() => EnemyPool.Instance.ActiveItems == 0).ContinueWith(ToNextState).Forget();
        }

        private void ToNextState()
        {
            CurrentStateMachine.ToNextState();
        }

        public void OnExit()
        {
            _instance.stopped -= OnDirectorStopped;
            _instance.Stop();

            Object.Destroy(_instance.gameObject);
        }
    }
}
