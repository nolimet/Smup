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

    bool autoCollectEnemiesOnStart = true;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        ActivePool = new List<EnemyStats>();
        InActivePool = new List<EnemyStats>();
    }

    void Start()
    {
        if (autoCollectEnemiesOnStart)
        {
            EnemyStats[] pl = FindObjectsOfType<EnemyStats>();
            foreach (EnemyStats p in pl)
            {
                if (p.gameObject.activeSelf)
                    ActivePool.Add(p);
                else
                    InActivePool.Add(p);

                p.transform.SetParent(transform);
            }
        }
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

            instance.onRemove(e);
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
