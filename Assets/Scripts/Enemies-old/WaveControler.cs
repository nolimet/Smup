﻿using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class WaveControler : MonoBehaviour {

    public delegate void WaveComplete();
    public event WaveComplete onWaveComplete;

    [SerializeField]
    int enemiesLeftInWave;

    [SerializeField]
    Vector2 waveStartOffset = Vector2.zero;
    public List<EnemyStats> currentEnemies;

    Dictionary<Vector2, char>[] paterns;

    void Start()
    {
        EnemyPool.instance.onRemove += EnemyPool_onRemove;
        currentEnemies = new List<EnemyStats>();

        WaveClass w = new WaveClass();

        Serialization.Load("Wave1", Serialization.fileTypes.wave, ref w);

        Dictionary<Vector3, char> tempDictonary = w.Convert();

        paterns = new Dictionary<Vector2, char>[(int)tempDictonary.Max(x => x.Key.z) + 1];

        foreach(Vector3 v in tempDictonary.Keys)
        {
            if (paterns[(int)v.z] == null)
                paterns[(int)v.z] = new Dictionary<Vector2, char>();
            paterns[(int)v.z].Add(v, tempDictonary[v]);
        }

        Debug.Log(paterns[0].Count());

        createWave();
    }

    void Destroy()
    {
        EnemyPool.instance.onRemove -= EnemyPool_onRemove;
    }

    private void EnemyPool_onRemove(EnemyStats Enemy)
    {
        enemiesLeftInWave--;
        currentEnemies.Remove(Enemy);
    }

    void Update()
    {
        if (enemiesLeftInWave <= 0)
            createWave();
    }

    Vector2 s = GameManager.screen.screenSize;
    public void createWave()
    {
        enemiesLeftInWave = 0;
        s = GameManager.screen.screenSize;
        ////for (int i = 0; i < 20; i++)
        ////{
        ////    addEnemy(new Vector3(s.x, (((s.y / 21f * i) + (s.y / 21f / 2f)) - (s.y / 2f))), EnemyStats.Type.SlowDownSpeedUp);
        ////}

        int w = Random.Range(0, paterns.Length);

        foreach (Vector2 v in paterns[w].Keys)
            addEnemy(v + waveStartOffset, (EnemyStats.Type)paterns[w][v]);
    }

    public void addEnemy(Vector3 pos, EnemyStats.Type Type)
    {
        enemiesLeftInWave++;
        EnemyStats e = EnemyPool.GetEnemy(Type);

        e.transform.position = pos;
        e.transform.rotation = Quaternion.identity;

        currentEnemies.Add(e);
    }


}
