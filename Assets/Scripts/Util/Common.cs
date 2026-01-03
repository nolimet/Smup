using System;
using JetBrains.Annotations;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// Common Utily libary. It contains fuctions that I used regulary or where very hard to figure out.
    /// </summary>
    public static class Common
    {
        /// <summary>
        /// Coverts a Angle to a directional Vector2
        /// </summary>
        /// <param name="angle">Angle in Degrees</param>
        /// <returns>Returns a vector to with values from -1 to 1 on each axis</returns>
        public static Vector2 AngleToVector(float angle)
        {
            Vector2 output;
            var radians = angle * Mathf.Deg2Rad;

            output = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians));
            return output;
        }

        /// <summary>
        /// Coverts a Vector2 into a Angle
        /// </summary>
        /// <param name="v2">A Vector to of which you want to know the direction in Degrees</param>
        /// <returns>Angle in degrees</returns>
        public static float VectorToAngle(Vector2 v2) => Mathf.Atan2(v2.y, v2.x) * 180f / Mathf.PI;

        /// <summary>
        /// Get the length of a vector
        /// </summary>
        /// <param name="obj">Vector of wich you want to know the length</param>
        /// <returns>Length of the vector</returns>
        public static float GetLength(this Vector2 obj) => Mathf.Sqrt(Mathf.Pow(obj.x, 2) + Mathf.Pow(obj.y, 2));

        /// <summary>
        /// Resizes an array to new size.
        /// This function can be used to resize multidimensional arrays
        /// </summary>
        /// <param name="arr">The array to be resized</param>
        /// <param name="newSizes">the new size of the array</param>
        /// <returns>The resized array</returns>
        public static Array ResizeArray([NotNull] Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new ArgumentException("arr must have the same number of dimensions " +
                                            "as there are elements in newSizes", "newSizes");

            var elementType = arr.GetType().GetElementType();
            if (elementType == null) throw new ArgumentException("arr must have an element type");

            var temp = Array.CreateInstance(elementType, newSizes);
            var length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            Array.ConstrainedCopy(arr, 0, temp, 0, length);
            return temp;
        }

        public static Vector2 GetSize(this Texture2D obj) => new(obj.width, obj.height);

        public static void ScaleSpriteToFitScreen(SpriteRenderer spriteRenderer, bool preserveAspect)
        {
            var newScale = Vector3.one;

            var width = spriteRenderer.sprite.bounds.size.x;
            var height = spriteRenderer.sprite.bounds.size.y;

            var worldScreenHeight = Camera.main!.orthographicSize * 2.0f;
            var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            newScale.x = worldScreenWidth / width;
            newScale.y = worldScreenHeight / height;

            if (preserveAspect)
            {
                if (newScale.x > newScale.y)
                    newScale.y = newScale.x;
                else
                    newScale.x = newScale.y;
            }

            spriteRenderer.transform.localScale = newScale;
        }

        public static Vector2 Round(this Vector2 v)
        {
            v.x = Mathf.Round(v.x);
            v.y = Mathf.Round(v.y);

            return v;
        }
    }
}
