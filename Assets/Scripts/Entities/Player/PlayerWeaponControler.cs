using System.Collections;
using Smup.Entities.Player.Weapons;
using Smup.Managers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Smup.Entities.Player
{
    public class PlayerWeaponControler : MonoBehaviour
    {
        public delegate void DelegateFireWeaon(WeaponType currentGun);

        public event DelegateFireWeaon OnFireWeapon;

        public static bool Firing;

        public Vector2 weaponOffset;

        private WeaponType _currentWeapon;

        public WeaponType CurrentWeapon
        {
            get => _currentWeapon;

            set
            {
                if (value != _currentWeapon)
                    MainWeapon = value switch
                    {
                        WeaponType.Cannon => new Cannon(),
                        WeaponType.Minigun => new MiniGun(),
                        WeaponType.Shotgun => new ShotGun(),
                        WeaponType.Granade => new Granade(),
                        _ => MainWeapon
                    };

                _currentWeapon = value;
            }
        }

        public IBaseWeapon MainWeapon;

        private Rigidbody2D _rigi;
        // Use this for initialization

        private InputActions.PlayerActions _playerInput;
        private InputActions.SwitchWeaponActions _switchWeapon;
        private Coroutine _shootRepeaterRoutine;

        private void Start()
        {
            _playerInput = GameManager.Input.Player;
            _rigi = GetComponent<Rigidbody2D>();
            MainWeapon = new Cannon();

            _playerInput.Shoot.performed += OnShootPressed;
            _playerInput.Shoot.Enable();

            _switchWeapon = GameManager.Input.SwitchWeapon;
            _switchWeapon.Enable();
            _switchWeapon.Cannon.performed += SwitchToCannon;
            _switchWeapon.Minigun.performed += SwitchToMinigun;
            _switchWeapon.Shotgun.performed += SwitchToShotgun;
            _switchWeapon.Granade.performed += SwitchToGranade;
        }

        private void SwitchToGranade(InputAction.CallbackContext ctx) => SwitchToWeapon(WeaponType.Granade);

        private void SwitchToShotgun(InputAction.CallbackContext ctx) => SwitchToWeapon(WeaponType.Shotgun);

        private void SwitchToMinigun(InputAction.CallbackContext ctx) => SwitchToWeapon(WeaponType.Minigun);

        private void SwitchToCannon(InputAction.CallbackContext ctx) => SwitchToWeapon(WeaponType.Cannon);

        private void SwitchToWeapon(WeaponType type)
        {
            CurrentWeapon = type switch
            {
                WeaponType.Cannon => type,
                WeaponType.Minigun when SaveDataManager.Upgrades.Minigun.Unlocked => type,
                WeaponType.Shotgun when SaveDataManager.Upgrades.Shotgun.Unlocked => type,
                WeaponType.Granade when SaveDataManager.Upgrades.Grenade.Unlocked => type,
                _ => _currentWeapon
            };
        }

        private void OnShootPressed(InputAction.CallbackContext _)
        {
            _shootRepeaterRoutine ??= StartCoroutine(ShootRepeater());
        }

        private IEnumerator ShootRepeater()
        {
            while (_playerInput.Shoot.IsPressed())
            {
                FireMain();
                yield return new WaitForSeconds(0.1f);
            }

            _shootRepeaterRoutine = null;
        }

        private void FireMain()
        {
            if (!GameManager.Stats.CanFire(MainWeapon.EnergyCost)) return;
            if (MainWeapon.TryShoot(gameObject, weaponOffset, GetAddedVelocity()))
            {
                GameManager.Stats.RemoveEnergy(MainWeapon.EnergyCost);
                OnFireWeapon?.Invoke(_currentWeapon);
            }
        }

        private Vector2 GetAddedVelocity() => _rigi.linearVelocity.x > 0 ? new Vector2(_rigi.linearVelocity.x, 0f) : Vector2.zero;

        private void OnDestroy()
        {
            _switchWeapon.Disable();
            _switchWeapon.Cannon.performed -= SwitchToCannon;
            _switchWeapon.Minigun.performed -= SwitchToMinigun;
            _switchWeapon.Shotgun.performed -= SwitchToShotgun;
            _switchWeapon.Granade.performed -= SwitchToGranade;

            _playerInput.Shoot.Disable();
            _playerInput.Shoot.performed -= OnShootPressed;
        }
    }
}
