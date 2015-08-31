using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveControler : MonoBehaviour {

    public delegate void WaveComplete();
    public event WaveComplete onWaveComplete;

    [SerializeField]
    int enemiesLeftInWave;
    public List<EnemyStats> currentEnemies;

    void Start()
    {
        EnemyPool.instance.onRemove += EnemyPool_onRemove;
        currentEnemies = new List<EnemyStats>();


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

    public void createWave()
    {
        Debug.Log("creating wave");
        Vector2 s = GameManager.screen.screenSize;
        for (int i = 0; i < 20; i++)
        {
            addEnemy(new Vector3(s.x, (((s.y / 21f * i) + (s.y / 21f / 2f)) - (s.y / 2f))), EnemyStats.Type.SlowDownSpeedUp);
        }
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
