using System;
using System.Threading;
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
        public StateMachineRuntime StateMachine { get; set; }

        [AssetsOnly]
        [SerializeField] private PlayableDirector director;

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
            _instance.stopped += ToNextState;
            _instance.Play();
        }

        private void ToNextState(object arg)
        {
            StateMachine.ToNextState();
        }

        public void OnExit()
        {
            _instance.stopped -= ToNextState;
            _instance.Stop();

            Object.Destroy(_instance.gameObject);
        }
    }
}
