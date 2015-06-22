using UnityEngine;
using System.Collections;

public class WaveControler : MonoBehaviour {

    int enemiesLeftInWave;

    public void Start()
    {
        EnemyPool.instance.onRemove += EnemyPool_onRemove;
    }

    public void Destroy()
    {
        EnemyPool.instance.onRemove -= EnemyPool_onRemove;
    }

    private void EnemyPool_onRemove(EnemyStats Enemy)
    {
        enemiesLeftInWave--;
    }

    public void addEnemy(Vector3 pos, EnemyStats.Type Type)
    {
        enemiesLeftInWave++;
        EnemyStats e = EnemyPool.GetEnemy(Type);

        e.transform.position = pos;
        e.transform.rotation = Quaternion.identity;
    }


}
