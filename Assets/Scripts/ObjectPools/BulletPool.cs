using System.Collections.Generic;
using System.Linq;
using Generic_Objects;
using UnityEngine;
using UnityEngine.Serialization;

namespace ObjectPools
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance;

        public delegate void RemoveObject(BulletGeneric item);

        public event RemoveObject OnRemove;

        [FormerlySerializedAs("ActivePool")] public List<BulletGeneric> activePool;
        [FormerlySerializedAs("InActivePool")] public List<BulletGeneric> inActivePool;

        public static void RemoveBullet(BulletGeneric item)
        {
            if (Instance && Instance.activePool.Contains(item))
            {
                Instance.activePool.Remove(item);
                Instance.inActivePool.Add(item);

                item.gameObject.SetActive(false);
            }
        }

        public static BulletGeneric GetBullet(BulletGeneric.Type type)
        {
            if (Instance)
            {
                BulletGeneric w;
                if (Instance.inActivePool.Any(e => e.weaponType == type))
                {
                    w = Instance.inActivePool.First(e => e.weaponType == type);

                    Instance.inActivePool.Remove(w);
                    Instance.activePool.Add(w);

                    w.gameObject.SetActive(true);
                }
                else
                {
                    var g = Instantiate(Resources.Load("Weapons/" + type.ToString()), Vector3.zero, Quaternion.identity) as GameObject;
                    w = g.GetComponent<BulletGeneric>();

                    Instance.activePool.Add(w);
                    w.transform.SetParent(Instance.transform);
                }

                return w;
            }

            return null;
        }

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void Update()
        {
            if (Instance == null)
                Instance = this;
        }
    }
}
