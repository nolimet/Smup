using System;
using UnityEngine;

namespace Util.IUpdatable
{
    public class BaseClass : MonoBehaviour, IUpdatable
    {
        // Use this for initialization
        private void Start()
        {
            UpdateManager.AddUpdateAble(this);
        }

        private void OnEnable()
        {
            UpdateManager.AddUpdateAble(this);
        }

        private void OnDisable()
        {
            UpdateManager.RemoveUpdateAble(this);
        }

        public virtual void Update()
        {
            Debug.Log(DateTime.Now);
        }
    }
}
