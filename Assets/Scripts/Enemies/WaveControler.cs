using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaveControler : MonoBehaviour {

    int enemiesLeftInWave;
    public List<EnemyStats> currentEnemies;

    void Start()
    {
        EnemyPool.instance.onRemove += EnemyPool_onRemove;
        createWave();
        currentEnemies = new List<EnemyStats>();
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
        for (int i = 0; i < 5; i++)
        {
            addEnemy(new Vector3(s.x, (((s.y / 6f * i) + (s.y / 6f / 2f)) - (s.y / 2f))), EnemyStats.Type.SlowDownSpeedUp);
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
