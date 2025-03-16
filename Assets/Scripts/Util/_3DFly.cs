using UnityEngine;

namespace Util
{
    [AddComponentMenu("Camera-Control/3DFly")]
    [RequireComponent(typeof(MouseLook))]
    public class _3DFly : MonoBehaviour
    {
        private bool _mouseRightDown;
        private bool _noRigiBody;

        private void Update()
        {
            var hor = Input.GetAxis("Horizontal");
            var ver = Input.GetAxis("Vertical");
            if (hor != 0 || ver != 0)
            {
                if (!_noRigiBody && GetComponent<Rigidbody>() != null)
                {
                    if (Input.GetAxis("Sprint") > 0)
                        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(hor, 0, ver) * 120f * Time.deltaTime);
                    else
                        GetComponent<Rigidbody>().AddRelativeForce(new Vector3(hor, 0, ver) * 60f * Time.deltaTime);
                }
                else
                {
                    _noRigiBody = true;
                    if (Input.GetAxis("Sprint") > 0)
                        transform.Translate(new Vector3(hor, 0, ver) * 12f * Time.deltaTime);
                    else
                        transform.Translate(new Vector3(hor, 0, ver) * 6f * Time.deltaTime);
                }
            }
            else
            {
                if (!_noRigiBody && GetComponent<Rigidbody>() != null)
                    GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            }
        }
    }
}
