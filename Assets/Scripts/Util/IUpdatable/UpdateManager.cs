using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Util.IUpdatable
{
    public class UpdateManager : MonoBehaviour
    {
        public static UpdateManager Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                _instance = FindAnyObjectByType<UpdateManager>();
                if (_instance)
                    return _instance;
                var g = new GameObject("Update Manager");
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
        [SerializeField] [ReadOnly] private int updatablesCount;

        private List<IUpdatable> _updateAbles;
        private List<IContinuesUpdateAble> _continuesUpdateables;

        private bool _paused = false;

        // Use this for initialization
        private void Awake()
        {
            _updateAbles = new List<IUpdatable>();
            _continuesUpdateables = new List<IContinuesUpdateAble>();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_updateAbles == null) _updateAbles = new List<IUpdatable>();

            updatablesCount = _updateAbles.Count;
            if (!_paused)
            {
                var tmp = _updateAbles.ToList();
                tmp.ForEach(i => i.Update());
            }

            _continuesUpdateables.ForEach(i => i.ContinuesUpdate());
        }

        public static void AddUpdateAble(IUpdatable i)
        {
            if (!Instance._updateAbles.Contains(i))
                Instance._updateAbles.Add(i);
        }

        public static void AddContinuesUpdateAble(IContinuesUpdateAble i)
        {
            if (!Instance._continuesUpdateables.Contains(i))
                Instance._continuesUpdateables.Add(i);
        }

        public static void RemoveUpdateAble(IUpdatable i)
        {
            if (Instance._updateAbles.Contains(i))
                Instance._updateAbles.Remove(i);
        }

        public static void RemoveContinuesUpdateAble(IContinuesUpdateAble i)
        {
            if (Instance._continuesUpdateables.Contains(i))
                Instance._continuesUpdateables.Remove(i);
        }
    }
}
