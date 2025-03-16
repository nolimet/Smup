using UnityEngine;

namespace Util
{
    public class MoveBoxScaler : MonoBehaviour
    {
        [SerializeField] private new Camera camera;

        public Vector3 screenSize = Vector3.zero;

        private void Awake()
        {
            var p1 = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

            transform.localScale = new Vector3(p1.x, p1.y, 1) * 2f;
            screenSize = new Vector3(p1.x, p1.y, 1) * 2f;
        }
    }
}
