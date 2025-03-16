using System;
using UnityEngine;

namespace Background
{
    public class BackGroundManager : MonoBehaviour
    {
        [SerializeField] private Layer[] layers;

        private void Update()
        {
            foreach (var l in layers)
            foreach (var t in l.parts)
            {
                if (t.localPosition.x < l.killPoint)
                    t.localPosition += new Vector3(l.startPoint * 2f, 0);
                t.localPosition -= new Vector3(l.speed * Time.deltaTime, 0);
            }
        }

        [Serializable]
        private class Layer
        {
            public string name;
            public Transform[] parts;
            public float speed, startPoint, killPoint;
        }
    }
}
