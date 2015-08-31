using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;


public class PickupPool : MonoBehaviour {

    public static PickupPool instance;
    public delegate void PickupActivty(Pickup obj);
    public event PickupActivty onRemove;

    List<Pickup> ActivePool, InActivePool;

    [SerializeField]
    bool collectPickupsAuto = true;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);

        ActivePool = new List<Pickup>();
        InActivePool = new List<Pickup>();
    }

    void Start()
    {
        if (collectPickupsAuto)
        {
            Pickup[] pl = FindObjectsOfType<Pickup>();
            foreach (Pickup p in pl)
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

    /// <summary>
    /// Create a collection of scrap
    /// </summary>
    /// <param name="pos">Where the cloud should be created on screen</param>
    /// <param name="Size">How large are the bounds of the cloud</param>
    /// <param name="count">How many bits of scrap are there in the cloud</param>
    /// <param name="Value">What is the combined value of the cloud</param>
    public static void CreateScrapCloud(Vector2 pos, Vector2 Size, int count, float Value)
    {
        Pickup p;
        Vector2 v2;
        for (int i = 0; i < count; i++)
        {
            p = Getpickup();
            v2 = new Vector2(Random.Range(Size.x / -2f, Size.x / 2f), Random.Range(Size.y / -2f, Size.y / 2f));
            p.transform.position = pos + v2;
            p.init(120, Value / count);
        }
    }

}
