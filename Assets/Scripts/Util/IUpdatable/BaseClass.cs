using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Util.Update
{
    public class BaseClass : MonoBehaviour, IUpdatable
    {
        // Use this for initialization
        void Start()
        {
            UpdateManager.addUpdateAble(this);
        }

        private void OnEnable()
        {
            UpdateManager.addUpdateAble(this);
        }

        private void OnDisable()
        {
            UpdateManager.removeUpdateAble(this);
        }

        public virtual void IUpdate()
        {
            Debug.Log(System.DateTime.Now);
        }
    }
}