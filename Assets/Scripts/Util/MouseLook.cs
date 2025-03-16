using UnityEngine;

namespace Util
{
    /// MouseLook rotates the transform based on the mouse delta.
    /// Minimum and Maximum values can be used to constrain the possible rotation
    /// To make an FPS style character:
    /// - Create a capsule.
    /// - Add the MouseLook script to the capsule.
    /// -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
    /// - Add FPSInputController script to the capsule
    /// -> A CharacterMotor and a CharacterController component will be automatically added.
    /// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
    /// - Add a MouseLook script to the camera.
    /// -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
    [AddComponentMenu("Camera-Control/Mouse Look")]
    public class MouseLook : MonoBehaviour
    {
        public enum RotationAxes
        {
            MouseXAndY = 0,
            MouseX = 1,
            MouseY = 2
        }

        public RotationAxes axes = RotationAxes.MouseXAndY;
        public float sensitivityX = 15F;
        public float sensitivityY = 15F;

        public float minimumX = -360F;
        public float maximumX = 360F;

        public float minimumY = -60F;
        public float maximumY = 60F;

        private float _rotationY;

        private bool _mouseDown;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1))
            {
                _mouseDown = true;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }

            if (Input.GetMouseButtonUp(1))
            {
                _mouseDown = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            if (_mouseDown)
            {
                if (axes == RotationAxes.MouseXAndY)
                {
                    var rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;
                    _rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    _rotationY = Mathf.Clamp(_rotationY, minimumY, maximumY);
                    transform.localEulerAngles = new Vector3(-_rotationY, rotationX, 0);
                }
                else if (axes == RotationAxes.MouseX)
                {
                    transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
                }
                else
                {
                    _rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
                    _rotationY = Mathf.Clamp(_rotationY, minimumY, maximumY);
                    transform.localEulerAngles = new Vector3(-_rotationY, transform.localEulerAngles.y, 0);
                }
            }
        }

        private void Start()
        {
            // Make the rigid body not change rotation
            if (GetComponent<Rigidbody>())
                GetComponent<Rigidbody>().freezeRotation = true;
        }
    }
}
