using Smup.Managers;
using UnityEngine;

namespace Smup.Entities.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMoveControler : MonoBehaviour
    {
        private const float Speed = 6f;
        private float _booster = 1.8f;
        private float _boostCost = 20; // persecond value

        private Rigidbody2D _rig;

        private InputActions.PlayerActions _playerInput;

        // Use this for initialization
        private void Start()
        {
            _rig = GetComponent<Rigidbody2D>();
            _playerInput = GameManager.Input.Player;

            _playerInput.PlayerMove.Enable();
            _playerInput.Boost.Enable();
        }

        // Update is called once per frame
        private void Update()
        {
            var dir = Vector2.zero;
            var input = _playerInput.PlayerMove.ReadValue<Vector2>();

            if (input.x > 0)
                dir.x = Speed * input.x;
            if (input.x < 0)
                dir.x = Speed * 0.7f * input.x;
            if (input.y != 0)
                dir.y = Speed * 0.75f * input.y;

            if (_playerInput.Boost.IsPressed() && dir.magnitude > float.Epsilon)
                if (GameManager.Stats.CanFire(_boostCost * Time.deltaTime))
                {
                    GameManager.Stats.RemoveEnergy(_boostCost * Time.deltaTime);
                    dir *= _booster;
                }

            _rig.linearVelocity = dir;
        }

        private void OnDestroy()
        {
            _playerInput.PlayerMove.Disable();
            _playerInput.Boost.Disable();
        }
    }
}
