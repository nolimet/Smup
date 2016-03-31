using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BulletPool : MonoBehaviour {

    public static BulletPool instance;
    public delegate void RemoveObject(BulletGeneric item);
    public event RemoveObject onRemove;

    public List<BulletGeneric> ActivePool, InActivePool;

    [SerializeField]
    private int noOfObjects = 0;

    public static void RemoveBullet(BulletGeneric item)
    {
        if (instance && instance.ActivePool.Contains(item))
        {
            instance.ActivePool.Remove(item);
            instance.InActivePool.Add(item);

            item.gameObject.SetActive(false);
        }    
    }

    public static BulletGeneric GetBullet(BulletGeneric.Type Type)
    {
        if (instance)
        {
            BulletGeneric w;
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
                w = g.GetComponent<BulletGeneric>();

                instance.ActivePool.Add(w);
                w.transform.SetParent(instance.transform);

                instance.noOfObjects++;
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
