using UnityEngine;

namespace Util
{
    /// <summary>
    /// Snaps a Object to a position on screen
    /// </summary>
    public class SnapToScreenPoint : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        public Vector3 screenPosition = Vector3.zero;

        private void Start()
        {
            if (camera == null)
                camera = Camera.main;

            DoMove();
        }

        private void Update()
        {
            DoMove();
        }

        private void DoMove()
        {
            var p1 = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            p1.Scale(screenPosition);
            p1.z = transform.position.z;
            transform.position = p1;
        }
    }
}
