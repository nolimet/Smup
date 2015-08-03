using UnityEngine;
using System.Collections;

namespace SerizableClasses
{
    [System.Serializable]
    public class _Vector3
    {
        public float x, y, z;
        public _Vector3(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public _Vector3(Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public Vector3 GetVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}