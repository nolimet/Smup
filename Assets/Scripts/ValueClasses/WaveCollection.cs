using System;
using System.Collections.Generic;

[Serializable]
// ReSharper disable once CheckNamespace
public class WaveCollection
{
    public List<WaveClass> dataCollection;
    public int WaveFormations => 0;

    public WaveCollection()
    {
        dataCollection = new List<WaveClass>();
    }
}
