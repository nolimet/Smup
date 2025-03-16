﻿using Managers;
using Player.Weapons;
using UnityEngine;
using Util;

namespace Player
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
            set
            {
                if (value != _currentWeapon)
                    switch (value)
                    {
                        case WeaponType.Cannon:
                            MainWeapon = new Cannon();
                            break;
                        case WeaponType.Minigun:
                            MainWeapon = new MiniGun();
                            break;

                        case WeaponType.Shotgun:
                            MainWeapon = new ShotGun();
                            break;

                        case WeaponType.Granade:
                            MainWeapon = new Granade();
                            break;
                    }

                _currentWeapon = value;
            }

            get => _currentWeapon;
        }

        public IBaseWeapon MainWeapon;

        private Rigidbody2D _rigi;
        // Use this for initialization

        private void Start()
        {
            _rigi = GetComponent<Rigidbody2D>();
            MainWeapon = new Cannon();
        }

        // Update is called once per frame
        private void Update()
        {
            if (Input.GetButton(Axis.Fire))
                FireMain();
            else
                Firing = false;

            SwitchWeapon();
        }

        private void SwitchWeapon()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) CurrentWeapon = WeaponType.Cannon;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                if (GameManager.Upgrades.miniGun.unlocked)
                    CurrentWeapon = WeaponType.Minigun;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                if (GameManager.Upgrades.shotgun.unlocked)
                    CurrentWeapon = WeaponType.Shotgun;
            if (Input.GetKeyDown(KeyCode.Alpha4))
                if (GameManager.Upgrades.grenade.unlocked)
                    CurrentWeapon = WeaponType.Granade;
        }

        private void FireMain()
        {
            if (!GameManager.Stats.CanFire(MainWeapon.EnergyCost))
                return;
            if (MainWeapon.TryShoot(gameObject, weaponOffset, GetAddedVelocity()))
            {
                GameManager.Stats.RemoveEnergy(MainWeapon.EnergyCost);
                OnFireWeapon?.Invoke(_currentWeapon);
            }
        }

        private Vector2 GetAddedVelocity()
        {
            Vector2 output;

            if (_rigi.linearVelocity.x > 0)
                output = new Vector2(_rigi.linearVelocity.x, 0f);
            else
                output = Vector2.zero;
            return output;
        }
    }
}
