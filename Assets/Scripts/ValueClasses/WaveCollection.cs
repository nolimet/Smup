using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WaveCollection
{
    public List<WaveClass> dataCollection;
    public int waveFormations
    {
        get { return 0; }
    }
    public WaveCollection()
    {
        dataCollection = new List<WaveClass>();
    }
}
