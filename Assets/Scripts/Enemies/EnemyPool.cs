using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;
    public delegate void EnemyActivty(EnemyStats Enemy);
    public static event EnemyActivty onRemove;

    List<EnemyStats> ActivePool, InActivePool;

    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        ActivePool = new List<EnemyStats>();
        InActivePool = new List<EnemyStats>();
    }

    void Update()
    {
        if(instance==null)
            instance = this;
    }

    public EnemyStats GetNewEnemy(EnemyStats.Type Type)
    {
        if (InActivePool.Any(e => e.type == Type))
        {
            return InActivePool.First(e => e.type == Type);
        }
        //TODO: create new enemy when there arn't enough in the pool. Make such a feature bullets too;

        return null;
    }
}
