using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class PickupPool : MonoBehaviour {

    public static PickupPool instance;
    public delegate void PickupActivty(Pickup obj);
    public event PickupActivty onRemove;

    List<Pickup> ActivePool, InActivePool;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        ActivePool = new List<Pickup>();
        InActivePool = new List<Pickup>();
    }

    void Update()
    {
        if (instance == null)
            instance = this;
    }

    public static void removePickup(Pickup p)
    {
        if (instance)
        {
            if (instance.ActivePool.Contains(p))
                instance.ActivePool.Remove(p);
            if (!instance.InActivePool.Contains(p))
                instance.InActivePool.Add(p);
            p.gameObject.SetActive(false);
        }
    }

    public static Pickup Getpickup()
    {
        if(instance)
        {
            Pickup p;
            if(instance.InActivePool.Count>0)
            {
                p = instance.InActivePool[0];
                instance.InActivePool.Remove(p);
                instance.ActivePool.Add(p);
                p.gameObject.SetActive(true);
            }
            else
            {
                GameObject g = Instantiate(Resources.Load("Pickups/Scrap"), Vector3.zero, Quaternion.identity) as GameObject;
                p = g.GetComponent<Pickup>();

                instance.ActivePool.Add(p);
                p.transform.SetParent(instance.transform, false);
            }
            return p;
        }
        return null;
    }


}
