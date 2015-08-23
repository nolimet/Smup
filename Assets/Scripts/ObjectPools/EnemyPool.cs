using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance;
    public delegate void EnemyActivty(EnemyStats Enemy);
    public event EnemyActivty onRemove;

    List<EnemyStats> ActivePool, InActivePool;

    void Awake()
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

    public static void RemoveEnemy(EnemyStats e)
    {
        if (instance)
        {
            if (instance.ActivePool.Contains(e))
                instance.ActivePool.Remove(e);
            if (!instance.InActivePool.Contains(e))
                instance.InActivePool.Add(e);
            e.gameObject.SetActive(false);
        }
    }

    public static EnemyStats GetEnemy(EnemyStats.Type Type)
    {
        if (instance)
        {
            EnemyStats e;
            if (instance.InActivePool.Any(i => i.type == Type))
            {
                e = instance.InActivePool.First(i => i.type == Type);
                instance.InActivePool.Remove(e);
                instance.ActivePool.Add(e);
                e.gameObject.SetActive(true);
            }
            else
            {
                GameObject g = Instantiate(Resources.Load("Enemies/" + Type.ToString()), Vector3.zero, Quaternion.identity) as GameObject;
                e = g.GetComponent<EnemyStats>();

                instance.ActivePool.Add(e);
                e.transform.SetParent(instance.transform,false);
            }

            e.gameObject.SendMessage("startBehaviours");
            return e;
        }
        return null;
    }
}
