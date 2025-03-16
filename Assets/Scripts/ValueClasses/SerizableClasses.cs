using System;

namespace ValueClasses
{
    [Serializable]
    public class Vector3
    {
        public float x, y, z;

        public Vector3(float x = 0f, float y = 0f, float z = 0f)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public Vector3(UnityEngine.Vector3 v3)
        {
            x = v3.x;
            y = v3.y;
            z = v3.z;
        }

        public UnityEngine.Vector3 GetVector3()
        {
            return new UnityEngine.Vector3(x, y, z);
        }
    }
}
