using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Util.Update
{
    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager instance
        {
            get
            {
                if (_instance)
                    return _instance;
                _instance = FindObjectOfType<UpdateManager>();
                if (_instance)
                    return _instance;
                GameObject g = new GameObject("Update Manager");
                _instance = g.AddComponent<UpdateManager>();
                if (_instance)
                    return _instance;

                Debug.LogError("NO GAMEMANGER FOUND! Check what is calling it");
                return null;
            }
        }
        private static UpdateManager _instance;

        /// <summary>
        /// Debug value
        /// </summary>
        [SerializeField, ReadOnly]
        private int updatablesCount;
        private List<IUpdatable> updateAbles;
        private List<IContinuesUpdateAble> continuesUpdateables;

        bool paused = false;

        // Use this for initialization
        private void Awake()
        {
            updateAbles = new List<IUpdatable>();
            continuesUpdateables = new List<IContinuesUpdateAble>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (updateAbles == null) updateAbles = new List<IUpdatable>();

            updatablesCount = updateAbles.Count;
            if (!paused)
            {
                List<IUpdatable> tmp = updateAbles.ToList();
                tmp.ForEach(i => i.IUpdate());
            }
            
            continuesUpdateables.ForEach(i => i.IContinuesUpdate());
        }
        
        public static void addUpdateAble(IUpdatable i)
        {
            if (!instance.updateAbles.Contains(i))
                instance.updateAbles.Add(i);
        }

        public static void addContinuesUpdateAble(IContinuesUpdateAble i)
        {
            if (!instance.continuesUpdateables.Contains(i))
                instance.continuesUpdateables.Add(i);
        }

        public static void removeUpdateAble(IUpdatable i)
        {
            if (instance.updateAbles.Contains(i))
                instance.updateAbles.Remove(i);
        }
        
        public static void removeContinuesUpdateAble(IContinuesUpdateAble i)
        {
            if (instance.continuesUpdateables.Contains(i))
                instance.continuesUpdateables.Remove(i);
        }
    }
}