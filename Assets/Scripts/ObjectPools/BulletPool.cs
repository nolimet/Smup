using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BulletPool : MonoBehaviour {

    public static BulletPool instance;
    public delegate void RemoveObject(WeaponBase item);
    public event RemoveObject onRemove;

    public List<WeaponBase> ActivePool, InActivePool;

    

    public static void RemoveBullet(WeaponBase item)
    {
        if (instance && instance.ActivePool.Contains(item))
        {
            instance.ActivePool.Remove(item);
            instance.InActivePool.Add(item);

            item.gameObject.SetActive(false);
        }    
    }

    public static WeaponBase GetBullet(WeaponTable.Weapons Type)
    {
        if (instance)
        {
            WeaponBase w;
            if (instance.InActivePool.Any(e => e.WeaponType == Type))
            {
                 w = instance.InActivePool.First(e => e.WeaponType == Type);

                 instance.InActivePool.Remove(w);
                 instance.ActivePool.Add(w);

                 w.gameObject.SetActive(true);
            }
            else
            {
                GameObject g = Instantiate(Resources.Load("Weapons/" + Type.ToString()), Vector3.zero, Quaternion.identity) as GameObject;
                w = g.GetComponent<WeaponBase>();

                instance.ActivePool.Add(w);
                w.transform.SetParent(instance.transform);
            }

            return w;
        }
        return null;
    }

    

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    public void Update()
    {
        if (instance == null)
            instance = this;
    }
    
}
