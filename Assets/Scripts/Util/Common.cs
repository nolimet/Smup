

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Util libary
/// Contains a quite a few usefull things like a serialization module, A event system and some hacks for the UI system
/// </summary>
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
            float radians = angle * Mathf.Deg2Rad;

            output = new Vector2((float)Mathf.Cos(radians), (float)Mathf.Sin(radians));
            return output;
        }

        /// <summary>
        /// Coverts a Vector2 into a Angle
        /// </summary>
        /// <param name="v2">A Vector to of which you want to know the direction in Degrees</param>
        /// <returns>Angle in degrees</returns>
        public static float VectorToAngle(Vector2 v2)
        {
            return Mathf.Atan2(v2.y, v2.x) * 180f / Mathf.PI;
        }

        /// <summary>
        /// Coverts bool into a int
        /// </summary>
        /// <param name="b">Bool that will be converted</param>
        /// <returns>0 if false| 1 if true</returns>
        public static int toInt(this bool b)
        {
            if (b)
                return 1;
            return 0;
        }

        /// <summary>
        /// Get the length of a vector
        /// </summary>
        /// <param name="obj">Vector of wich you want to know the length</param>
        /// <returns>Length of the vector</returns>
        public static float getLength(this Vector2 obj)
        {
            return Mathf.Sqrt(Mathf.Pow(obj.x, 2) + Mathf.Pow(obj.y, 2));
        }

        /// <summary>
        /// Gets the bounds of a object including it's childeren
        /// </summary>
        /// <param name="obj">object's transform that contains the childeren</param>
        /// <returns>Bounds including Parents childeren</returns>
        public static Bounds getChildBounds(this Transform obj)
        {
            Bounds bounds;
            // First find a center for your bounds.
            Vector3 center = Vector3.zero;
            foreach (Transform child in obj.transform)
            {
                center += child.gameObject.GetComponent<SpriteRenderer>().bounds.center;
            }
            center /= obj.transform.childCount; //center is average center of children

            //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
            bounds = new Bounds(center, Vector3.zero);

            foreach (Transform child in obj.transform)
            {
                bounds.Encapsulate(child.gameObject.GetComponent<SpriteRenderer>().bounds);
            }
            return bounds;
        }

        /// <summary>
        /// Gets the bounds of a object including it's childeren
        /// </summary>
        /// <param name="obj">object's transform that contains the childeren</param>
        /// <param name="ignorNameTag">Object names to ignor usefull to create mask</param>
        /// <returns>Bounds including Parents childeren</returns>
        public static Bounds getChildBounds(this Transform obj, string ignorNameTag)
        {
            Bounds bounds;

            //so i don't do useless checks
            if (ignorNameTag == "")
            {
                // First find a center for your bounds.
                Vector3 center = Vector3.zero;
                foreach (Transform child in obj.transform)
                {
                    center += child.gameObject.GetComponent<SpriteRenderer>().bounds.center;
                }
                center /= obj.transform.childCount; //center is average center of children

                //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
                bounds = new Bounds(center, Vector3.zero);

                foreach (Transform child in obj.transform)
                {
                    bounds.Encapsulate(child.gameObject.GetComponent<SpriteRenderer>().bounds);
                }
            }
            //
            else
            {
                ignorNameTag = ignorNameTag.ToLower();
                // First find a center for your bounds.
                Vector3 center = Vector3.zero;
                int i = 0;
                foreach (Transform child in obj.transform)
                {
                    if (!child.gameObject.name.ToLower().Contains(ignorNameTag))
                    {
                        //Debug.Log(child.name);
                        center += child.gameObject.GetComponent<SpriteRenderer>().bounds.center;
                        i++;
                    }
                }
                center /= i; //center is average center of children

                //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
                bounds = new Bounds(center, Vector3.zero);

                foreach (Transform child in obj.transform)
                {
                    if (!child.name.ToLower().Contains(ignorNameTag))
                        bounds.Encapsulate(child.gameObject.GetComponent<SpriteRenderer>().bounds);
                }
            }
            return bounds;
        }

        /// <summary>
        /// Gets the bounds of a object including it's childeren
        /// </summary>
        /// <param name="obj">object's transform that contains the childeren</param>
        /// <param name="IgnorNameTags">A set of name part to ignor used to create more complex masks</param>
        /// <returns>Bounds including Parents childeren</returns>
        public static Bounds getChildBounds(this Transform obj, string[] IgnorNameTags)
        {
            if (IgnorNameTags.Length == 0)
                return obj.getChildBounds();

            Bounds bounds;
            Vector3 center = Vector3.zero;
            int i = 0;
            string n;


            for (int j = 0; j < IgnorNameTags.Length; j++)
            {
                IgnorNameTags[j] = IgnorNameTags[j].ToLower();
            }

            // First find a center for your bounds.s
            foreach (Transform child in obj.transform)
            {
                n = child.gameObject.name.ToLower();
                if (IgnorNameTags.Any(str => n.Contains(str)))
                {
                    //Debug.Log(child.name);
                    center += child.gameObject.GetComponent<SpriteRenderer>().bounds.center;
                    i++;
                }
            }
            center /= i; //center is average center of children

            //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
            bounds = new Bounds(center, Vector3.zero);

            foreach (Transform child in obj.transform)
            {
                n = child.gameObject.name.ToLower();
                if (IgnorNameTags.Any(str => n.Contains(str)))
                    bounds.Encapsulate(child.gameObject.GetComponent<SpriteRenderer>().bounds);
            }

            return bounds;
        }

        /// <summary>
        /// Input array to return combined bounds of array
        /// </summary>
        /// <param name="objs">objects to get combined bound of</param>
        /// <returns>Combined bound</returns>
        public static Bounds getBounds(this Transform[] objs)
        {
            Bounds bounds;
            // First find a center for your bounds.
            Vector3 center = Vector3.zero;
            foreach (Transform child in objs)
            {
                center += child.gameObject.GetComponent<SpriteRenderer>().bounds.center;
            }
            center /= objs.Length; //center is average center of children

            //Now you have a center, calculate the bounds by creating a zero sized 'Bounds', 
            bounds = new Bounds(center, Vector3.zero);

            foreach (Transform child in objs)
            {
                bounds.Encapsulate(child.gameObject.GetComponent<SpriteRenderer>().bounds);
            }
            return bounds;
        }
        public static Bounds getBounds(this Transform obj)
        {
            return obj.gameObject.GetComponent<SpriteRenderer>().bounds;
        }
        /// <summary>
        /// Draw the bounds of a object
        /// </summary>
        /// <param name="b">Bounds that will be drawn</param>
        public static void DrawBounds(Bounds b)
        {
            Debug.DrawLine(b.max, new Vector3(b.max.x, b.min.y));
            Debug.DrawLine(new Vector3(b.max.x, b.min.y), b.min);
            Debug.DrawLine(b.min, new Vector3(b.min.x, b.max.y));
            Debug.DrawLine(new Vector3(b.min.x, b.max.y), b.max);
            Debug.DrawLine(b.max, b.min, Color.red);
            Debug.DrawLine(new Vector3(b.min.x, b.max.y), new Vector3(b.max.x, b.min.y), Color.red);
        }

        /// <summary>
        /// Check if bounds are inside the target
        /// </summary>
        /// <param name="bounds"></param>
        /// <param name="target">Object contained in other bound</param>
        /// <returns></returns>
        public static bool ContainBounds(this Bounds bounds, Bounds target)
        {
            return bounds.Contains(target.min) && bounds.Contains(target.max);
        }

        public static float CalculateJumpVerticalSpeed(float targetJumpHeight)
        {
            // From the jump height and gravity we deduce the upwards speed 
            // for the character to reach at the apex.
            Debug.Log(2f * targetJumpHeight * Physics2D.gravity.y);
            return Mathf.Sqrt(2f * targetJumpHeight * Physics2D.gravity.y);
        }

        /// <summary>
        /// breaks up a number into pieces and return the value at the point of the power
        /// </summary>
        /// <param name="number">should not be seen. The number to exstract from</param>
        /// <param name="power">wat what point do i need to take the return number. In powers of 10( 3, 2 ,1 ,0 )</param>
        /// <returns>a number from 0-9</returns>
        public static int getNumbAt(this int number, int power)
        {
            return number / (int)Mathf.Pow(10, power) % 10;
        }

        /// <summary>
        /// Resizes an array to new size.
        /// This function can be used to resize multidimensional arrays
        /// </summary>
        /// <param name="arr">The array to be resized</param>
        /// <param name="newSizes">the new size of the array</param>
        /// <returns>The resized array</returns>
        public static System.Array ResizeArray(System.Array arr, int[] newSizes)
        {
            if (newSizes.Length != arr.Rank)
                throw new System.ArgumentException("arr must have the same number of dimensions " +
                                            "as there are elements in newSizes", "newSizes");

            var temp = System.Array.CreateInstance(arr.GetType().GetElementType(), newSizes);
            int length = arr.Length <= temp.Length ? arr.Length : temp.Length;
            System.Array.ConstrainedCopy(arr, 0, temp, 0, length);
            return temp;
        }

        public static Vector2 GetSize(this Texture2D obj)
        {
            return new Vector2(obj.width, obj.height);
        }


        public static void ScaleSpriteToFitScreen(SpriteRenderer spriteRenderer, bool preserveAspect)
        {

            Vector3 newScale = Vector3.one;

            float width = spriteRenderer.sprite.bounds.size.x;
            float height = spriteRenderer.sprite.bounds.size.y;

            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

            newScale.x = worldScreenWidth / width;
            newScale.y = worldScreenHeight / height;

            if (preserveAspect)
            {
                if(newScale.x> newScale.y)
                {
                    newScale.y = newScale.x;
                }
                else
                {
                    newScale.x = newScale.y;
                }
            }

            spriteRenderer.transform.localScale = newScale;
        }

        /// <summary>
        /// Rotate a Vector2 to a angle
        /// </summary>
        /// <param name="v"></param>
        /// <param name="degrees">The angle to rotate it to</param>
        /// <returns>Rotated Vector2</returns>
        public static Vector2 Rotate(this Vector2 v, float degrees)
        {
            /*
            Actual Calculation

            float radians = degrees * Mathf.Deg2Rad;
             float sin = Mathf.Sin(radians);
             float cos = Mathf.Cos(radians);
         
             float tx = v.x;
             float ty = v.y;
 
             return new Vector2(cos * tx - sin * ty, sin * tx + cos * ty);
            */
            return Quaternion.Euler(0, 0, degrees) * v;
        }

        /// <summary>
        /// Rotates a Vector2 to a Quaternion
        /// </summary>
        /// <param name="v"></param>
        /// <param name="rotation">The rotation used to to calculate the new vector</param>
        /// <returns>Rotated Vector2</returns>
        public static Vector2 Rotate(this Vector2 v, Quaternion rotation)
        {
            return new Quaternion(0,0,rotation.z,rotation.w) * v;
        }
    }
}