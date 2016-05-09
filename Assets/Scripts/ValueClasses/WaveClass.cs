using UnityEngine;
using SerizableClasses;
using System.Collections.Generic;

[System.Serializable]
public class WaveClass
{
    public char[,,] waves = new char[0,0,0];

    public WaveClass()
    {
        waves = new char[0, 0, 0];
    }

    public WaveClass(Dictionary<Vector3,char> points, Vector3 arraySize)
    {
        waves = new char[(int)arraySize.z, (int)arraySize.y + 1, (int)arraySize.x + 1];
        setToNull();

        foreach(Vector3 v in points.Keys)
        {
            waves[(int)v.z, (int)v.y, (int)v.x] = points[v];
        }

        
    }

    public Dictionary<Vector3,char> Convert()
    {
      Dictionary<Vector3, char> output = new Dictionary<Vector3, char>();
        for (int z = 0; z < waves.GetLength(0); z++)
        {
            for (int y = 0; y < waves.GetLength(1); y++)
            {
                for (int x = 0; x < waves.GetLength(2); x++)
                {
                    if(waves[z, y, x]!= '\0')
                    output.Add(new Vector3(x, y, z), waves[z, y, x]);
                }
            }
        }

        return output;
    }

    void setToNull()
    {
        for (int z = 0; z < waves.GetLength(0); z++)
            for (int y = 0; y < waves.GetLength(1); y++)
                for (int x = 0; x < waves.GetLength(2); x++)
                    waves[z, y, x] = '\0';
    }
}
