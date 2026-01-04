using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Helpers;
using Pools;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using Util.StateMachine;
using World.Waves.Data;
using World.Waves.Interfaces;

namespace World.Waves.States
{
    [Serializable]
    public class SpawnPatternState : ISequenceElement
    {
        public StateMachineRuntime CurrentStateMachine { get; set; }

        [SerializeField] private bool WaitForZeroEnemies;

        [HorizontalGroup]
        [SerializeField] private Transform dynamicSpawnOffset;

        [HorizontalGroup] [ShowIf(nameof(DynamicOffsetIsSet))]
        [SerializeField] private Vector2 spawnOffset;

        [OnValueChanged(nameof(SpawnPatternChanged))]
        [SerializeField] private SpawnPattern spawnPattern;

        [ShowIf(nameof(ShowTypeOverrides))]
        [OdinSerialize] [ShowInInspector] [TableList]
        private List<PartOverride> _typeOverrides = new();

        private void SpawnPatternChanged() => _typeOverrides.Clear();
        private bool ShowTypeOverrides() => spawnPattern && spawnPattern.parts.Count > 0;
        private bool DynamicOffsetIsSet() => dynamicSpawnOffset;

        public void OnEnter()
        {
            Vector2 origin = dynamicSpawnOffset.position + (Vector3)spawnOffset;

            foreach (var part in spawnPattern.parts)
            {
                var enemyTypeName = _typeOverrides.FirstOrDefault(x => x.key == part.typeName)?.value;
                if (string.IsNullOrWhiteSpace(enemyTypeName)) enemyTypeName = part.typeName;

                foreach (var partPosition in part.positions)
                {
                    var enemy = EnemyPool.Instance.GetObject(enemyTypeName);
                    enemy.transform.position = origin + partPosition;
                }
            }

            if (WaitForZeroEnemies)
                UniTask.WaitUntil(() => EnemyPool.Instance.ActiveItems == 0).ContinueWith(CurrentStateMachine.ToNextState).Forget();
            else
                CurrentStateMachine.ToNextState();
        }

        public void OnExit() { }

        private class PartOverride
        {
            [ValueDropdown(nameof(GetEnemyTypes))]
            public string key;

            [ValueDropdown(nameof(GetEnemyTypes))]
            public string value;

            private IEnumerable<string> GetEnemyTypes() => EnemyTypeHelper.GetEnemyTypes();
        }
    }
}
