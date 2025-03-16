using UnityEngine;
using UnityEngine.Serialization;

namespace Util
{
    /// <summary>
    /// Snaps a Object to a position on screen
    /// </summary>
    public class SnapToScreenPoint : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        /// <summary>
        /// Position on screen
        /// Value should be between -100 and 100
        /// </summary>
        [Tooltip("Should be value between -100 and 100")]
        public Vector3 screenPosition = Vector3.zero;

        /// <summary>
        /// Sprite size in pixels to make sure it's does not get streched 
        /// </summary>
        [FormerlySerializedAs("StartSize")] [Tooltip("Sprite's Size in pixel used to make sure it's scaled correctly")]
        public Vector2 startSize = Vector2.one;

        [FormerlySerializedAs("PivotPosition")] public Vector2 pivotPosition = Vector2.one / 2f;

        /// <summary>
        /// Should the object be moved in this direction
        /// </summary>
        [FormerlySerializedAs("Vertical")] [SerializeField] [Tooltip("Should Object be moved in this direction?")]
        private bool vertical;

        /// <summary>
        /// Should the object be moved in this direction
        /// </summary>
        [FormerlySerializedAs("Horizontal")] [SerializeField] [Tooltip("Should Object be moved in this direction?")]
        private bool horizontal;

        [FormerlySerializedAs("UsePivot")] [SerializeField] [Tooltip("use center point")]
        private bool usePivot;

        private void Start()
        {
            if (camera == null)
                camera = Camera.main;

            screenPosition /= 100f;
            startSize /= 100f;

            DoMove();

            //if (GetComponent<SpriteRenderer>())
            //{
            //    SpriteRenderer spr = GetComponent<SpriteRenderer>();
            //    StartSize = spr.bounds.size;
            //    StartSize.x += transform.localScale.x;
            //    StartSize.y += transform.localScale.y;
            //}
        }

        private void Update()
        {
            DoMove();
        }

        private void DoMove()
        {
            var p1 = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            p1.z = transform.position.z;
            p1.y *= screenPosition.y;
            p1.x *= screenPosition.x;

            // p1 *= 2f;
            if (!vertical)
                p1.y = transform.position.y;

            if (!horizontal)
                p1.x = transform.position.x;

            if (usePivot)
            {
                if (vertical)
                    if (screenPosition.y > 0)
                        p1.y += transform.localScale.y * pivotPosition.y * startSize.y;
                    else
                        p1.y -= transform.localScale.y * pivotPosition.y * startSize.y;

                if (horizontal)
                    if (screenPosition.x > 0)
                        p1.x += transform.localScale.x * pivotPosition.x * startSize.x;
                    else
                        p1.x -= transform.localScale.x * pivotPosition.x * startSize.x;
            }

            transform.position = p1;
        }
    }
}
