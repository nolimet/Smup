using System;
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
        [SerializeField] private SpawnPattern spawnPattern;
        
        [SerializeField] private Vector2 spawnOffset;
        [SerializeField] private Transform dynamicSpawnOffset;

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
           
        }
    }
}
