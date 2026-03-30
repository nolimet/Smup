using System.Collections.Generic;
using UnityEngine;

namespace Smup.World.Waves.Data
{
    public class SpawnPattern : ScriptableObject
    {
        public List<SpawnPatternPart> parts = new();
    }
}
