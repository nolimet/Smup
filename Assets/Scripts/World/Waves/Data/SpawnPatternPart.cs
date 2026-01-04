using System;
using Helpers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace World.Waves.Data
{
    [Serializable]
    public class SpawnPatternPart
    {
        [FormerlySerializedAs("enemyId")] [ValueDropdown(nameof(Enemies))] public string typeName;
        public Vector2[] positions;

        private string[] Enemies => EnemyTypeHelper.GetEnemyTypes();
    }
}
