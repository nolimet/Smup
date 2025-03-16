using UnityEngine;
using UnityEngine.Serialization;

namespace Util
{
    /// <summary>
    /// 3d version of Scale to ScreenSize
    /// </summary>
    public class ScaleToCameraView : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [FormerlySerializedAs("ScaleSize")] [SerializeField] private Vector2 scaleSize = Vector2.zero;

        private void Start()
        {
            var tmp = scaleSize / 100f;

            //Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.width / 2f, transform.position.z - cam.transform.position.z));

            var p1 = cam.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - cam.transform.position.z));
            var p2 = cam.ScreenToWorldPoint(new Vector3(tmp.x * Screen.width, tmp.y * Screen.height, transform.position.z - cam.transform.position.z));

            var newScale = p1 - p2;

            if (newScale.x < 0)
                newScale.x = -newScale.x;
            if (newScale.y < 0)
                newScale.y = -newScale.y;
            if (newScale.z < 0)
                newScale.z = -newScale.z;

            transform.localScale = newScale;
            // transform.position = pos;
            transform.rotation = cam.transform.rotation;
        }

        private void OnDrawGizmosSelected()
        {
            var tmp = scaleSize / 100f;

            var p1 = cam.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - cam.transform.position.z));
            var p2 = cam.ScreenToWorldPoint(new Vector3(tmp.x * Screen.width, tmp.y * Screen.height, transform.position.z - cam.transform.position.z));

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p1, 0.4F);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(p2, 0.4F);
        }
    }
}
