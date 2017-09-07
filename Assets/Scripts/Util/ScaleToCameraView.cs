using UnityEngine;
using System.Collections;

namespace Util
{
    /// <summary>
    /// 3d version of Scale to ScreenSize
    /// </summary>
    public class ScaleToCameraView : MonoBehaviour
    {
        [SerializeField]
        Camera cam = null;
        [SerializeField]
        Vector2 ScaleSize = Vector2.zero;

        void Start()
        {
            Vector2 tmp = ScaleSize / 100f;

            //Vector3 pos = cam.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.width / 2f, transform.position.z - cam.transform.position.z));

            Vector3 p1 = cam.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - cam.transform.position.z));
            Vector3 p2 = cam.ScreenToWorldPoint(new Vector3(tmp.x * Screen.width, tmp.y * Screen.height, transform.position.z - cam.transform.position.z));

            Vector3 newScale = p1 - p2;

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

        void OnDrawGizmosSelected()
        {
            Vector2 tmp = ScaleSize / 100f;

            Vector3 p1 = cam.ScreenToWorldPoint(new Vector3(0, 0, transform.position.z - cam.transform.position.z));
            Vector3 p2 = cam.ScreenToWorldPoint(new Vector3(tmp.x * Screen.width, tmp.y * Screen.height, transform.position.z - cam.transform.position.z));

            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(p1, 0.4F);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(p2, 0.4F);
        }
    }
}