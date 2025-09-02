using Managers;
using UnityEngine;
using Util;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMoveControler : MonoBehaviour
    {
        private const float Speed = 6f;
        private float _booster = 1.8f;
        private float _boostCost = 20; // persecond value

        private Rigidbody2D _rig;

        // Use this for initialization
        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
        }

        // Update is called once per frame
        private void Update()
        {
            var dir = Vector2.zero;

            if (Input.GetAxis(Axis.Horizontal) > 0)
                dir.x = Speed * Input.GetAxis(Axis.Horizontal);
            if (Input.GetAxis(Axis.Horizontal) < 0)
                dir.x = Speed * 0.7f * Input.GetAxis(Axis.Horizontal);
            if (Input.GetAxis(Axis.Vertical) != 0)
                dir.y = Speed * 0.75f * Input.GetAxis(Axis.Vertical);

            if (Input.GetAxis(Axis.Boost) != 0 && dir.magnitude > float.Epsilon)
                if (GameManager.Stats.CanFire(_boostCost * Time.deltaTime))
                {
                    GameManager.Stats.RemoveEnergy(_boostCost * Time.deltaTime);
                    dir *= _booster;
                }

            _rig.linearVelocity = dir;
        }
    }
}
