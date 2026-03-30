using System;
using Sirenix.OdinInspector;
using Smup.Helpers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Smup.World.Waves.Data
{
    [Serializable]
    public class SpawnPatternPart
    {
        [FormerlySerializedAs("enemyId")] [ValueDropdown(nameof(Enemies))] public string typeName;
        public Vector2[] positions;

        private string[] Enemies => EnemyTypeHelper.GetEnemyTypes();
    }
}
