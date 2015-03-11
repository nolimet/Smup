using UnityEngine;
using System.Collections;

public class MathHelper
{

    public static Vector2 AngleToVector(float angle)
    {
        Vector2 output;
        float radians = angle * Mathf.Deg2Rad;

        output = new Vector2((float)Mathf.Cos(radians), (float)Mathf.Sin(radians));
        return output;
    }
}
