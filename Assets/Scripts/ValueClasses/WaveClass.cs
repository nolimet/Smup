using System;
using System.Collections.Generic;

namespace ValueClasses
{
    [Serializable]
// ReSharper disable once CheckNamespace
    public class WaveClass
    {
        // ReSharper disable once InconsistentNaming
        public readonly char[,,] waves; //TODO remake data

        public WaveClass() => waves = new char[0, 0, 0];

        public WaveClass(char[,,] waves) => this.waves = waves;

        public WaveClass(Dictionary<UnityEngine.Vector3, char> points, UnityEngine.Vector3 arraySize)
        {
            waves = new char[(int)arraySize.z, (int)arraySize.y + 1, (int)arraySize.x + 1];
            SetToNull();

            foreach (var v in points.Keys) waves[(int)v.z, (int)v.y, (int)v.x] = points[v];
        }

        public Dictionary<UnityEngine.Vector3, char> Convert()
        {
            var output = new Dictionary<UnityEngine.Vector3, char>();
            for (var z = 0; z < waves.GetLength(0); z++)
            for (var y = 0; y < waves.GetLength(1); y++)
            for (var x = 0; x < waves.GetLength(2); x++)
                if (waves[z, y, x] != '\0')
                    output.Add(new UnityEngine.Vector3(x, y, z), waves[z, y, x]);

            return output;
        }

        private void SetToNull()
        {
            for (var z = 0; z < waves.GetLength(0); z++)
            for (var y = 0; y < waves.GetLength(1); y++)
            for (var x = 0; x < waves.GetLength(2); x++)
                waves[z, y, x] = '\0';
        }
    }
}
