using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;

namespace UpgradeSystem
{
    [Serializable]
    public class UpgradeData
    {
        public long upgradeCurrency;

        //ScrapGetting
        [ShowInInspector] public readonly Upgradable<int> ScrapCollectionRange = new(-1, l => l, l => 900 * Math.Pow(1.3, l));
        [ShowInInspector] public readonly Upgradable<int> ScrapCollectionSpeed = new(10, l => l, l => 650 * Math.Pow(1.3, l));
        [ShowInInspector] public readonly Upgradable<int> ScrapConversionRate = new(-1, l => l, l => 650 * Math.Pow(1.5, l));

        //Hull
        [ShowInInspector] public readonly Upgradable<int> HullUpgradeLevel = new(10, l => l, l => 50 * Math.Pow(2, l));
        [ShowInInspector] public readonly Upgradable<int> ArmorUpgradeLevel = new(-1, l => l, l => 100 * Math.Pow(1.4, l));

        //Shield
        [ShowInInspector] public readonly Upgradable<bool> UnlockedShield = new(1, l => l > 0, _ => 1200);
        [ShowInInspector] public readonly Upgradable<int> ShieldGeneratorLevel = new(-1, l => l, l => 1300 * Math.Pow(1.2, l));
        [ShowInInspector] public readonly Upgradable<int> ShieldCapacitorLevel = new(-1, l => l, l => 1800 * Math.Pow(1.2f, l));

        //weaponUpgrades
        [ShowInInspector] public readonly CommonWeaponUpgrade Cannon = new
        (
            null,
            new Upgradable<int>(-1, l => l, l => 350 * Math.Pow(1.4, l)),
            new Upgradable<int>(5, l => l, l => 200 * Math.Pow(1.2, l)),
            new Upgradable<int>(20, l => l, l => 475 * Math.Pow(1.7, l)),
            null
        );
        [ShowInInspector] public readonly CommonWeaponUpgrade MiniGun = new
        (
            new Upgradable<bool>(1, l => l == 1, _ => 700),
            new Upgradable<int>(-1, l => l, l => 4000 * Math.Pow(1.1, l)),
            new Upgradable<int>(10, l => l, l => 3000 * Math.Pow(1.3, l)),
            new Upgradable<int>(20, l => l, l => 2500 * Math.Pow(1.2, l)),
            null
        );
        [ShowInInspector] public readonly CommonWeaponUpgrade Shotgun = new
        (
            new Upgradable<bool>(1, l => l == 1, l => 750),
            new Upgradable<int>(-1, l => l, l => 1250 * Math.Pow(1.3, l)),
            new Upgradable<int>(10, l => l, l => 1425 * Math.Pow(1.8, l)),
            new Upgradable<int>(6, l => l, l => 750 * Math.Pow(1.3, l)),
            null
        );
        [ShowInInspector] public readonly CommonWeaponUpgrade Grenade = new
        (
            new Upgradable<bool>(1, l => l == 1, l => 5000),
            new Upgradable<int>(-1, l => l, l => 1250 * Math.Pow(1.3, l)),
            null,
            new Upgradable<int>(20, l => l, l => 4000 * Math.Pow(2, l)),
            new Upgradable<int>(30, l => l, l => 1500 * Math.Pow(1.7, l))
        );

        public byte[] GetBytes()
        {
            var bytes = new List<byte>();
            var unfilteredFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);

            bytes.AddRange(BitConverter.GetBytes(upgradeCurrency));

            foreach (var fieldInfo in unfilteredFields)
            {
                var value = fieldInfo.GetValue(this);
                if (value is Upgradable upgradable)
                {
                    bytes.AddRange(upgradable.ToBytes());
                }
                else if (value is CommonWeaponUpgrade weaponUpgrade)
                {
                    bytes.AddRange(weaponUpgrade.GetBytes());
                }
            }

            return bytes.ToArray();
        }

        public void ApplyBytes(byte[] bytes)
        {
            var byteSpan = bytes.AsSpan();
            var offset = 8;
            var unfilteredFields = GetType().GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
            upgradeCurrency = BitConverter.ToInt64(byteSpan.Slice(0, 8));

            foreach (var fieldInfo in unfilteredFields)
            {
                var value = fieldInfo.GetValue(this);
                if (value is Upgradable upgradable)
                {
                    upgradable.ApplyBytes(byteSpan.Slice(offset, Upgradable.ByteSize));
                    offset += Upgradable.ByteSize;
                }
                else if (value is CommonWeaponUpgrade weaponUpgrade)
                {
                    weaponUpgrade.ApplyBytes(byteSpan.Slice(offset, CommonWeaponUpgrade.ByteSize));
                    offset += CommonWeaponUpgrade.ByteSize;
                }
            }
        }
    }

    [Serializable]
    public struct CommonWeaponUpgrade
    {
        public const int ByteSize = 5 * Upgradable.ByteSize;

        [ShowInInspector] public readonly Upgradable<bool> Unlocked;
        [ShowInInspector] public readonly Upgradable<int> Damage;
        [ShowInInspector] public readonly Upgradable<int> Accuracy;
        [ShowInInspector] public readonly Upgradable<int> FireRate;
        [ShowInInspector] public readonly Upgradable<int> Fragments;

        public CommonWeaponUpgrade(Upgradable<bool> unlocked, Upgradable<int> damage, Upgradable<int> accuracy, Upgradable<int> fireRate, Upgradable<int> fragments)
        {
            Unlocked = unlocked;
            Damage = damage;
            Accuracy = accuracy;
            FireRate = fireRate;
            Fragments = fragments;
        }

        public byte[] GetBytes()
        {
            List<byte> bytes = new();
            bytes.AddRange(Unlocked?.ToBytes() ?? Upgradable.Empty);
            bytes.AddRange(Damage?.ToBytes() ?? Upgradable.Empty);
            bytes.AddRange(Accuracy?.ToBytes() ?? Upgradable.Empty);
            bytes.AddRange(FireRate?.ToBytes() ?? Upgradable.Empty);
            bytes.AddRange(Fragments?.ToBytes() ?? Upgradable.Empty);
            return bytes.ToArray();
        }

        public void ApplyBytes(ReadOnlySpan<byte> data)
        {
            if (ByteSize != data.Length)
            {
                throw new Exception($"Expected {ByteSize}bytes got {data.Length}bytes");
            }

            Unlocked?.ApplyBytes(data.Slice(Upgradable.ByteSize * 0, Upgradable.ByteSize));
            Damage?.ApplyBytes(data.Slice(Upgradable.ByteSize * 1, Upgradable.ByteSize));
            Accuracy?.ApplyBytes(data.Slice(Upgradable.ByteSize * 2, Upgradable.ByteSize));
            FireRate?.ApplyBytes(data.Slice(Upgradable.ByteSize * 3, Upgradable.ByteSize));
            Fragments?.ApplyBytes(data.Slice(Upgradable.ByteSize * 4, Upgradable.ByteSize));
        }
        //Not implemented yet
        /*public bool unlockedTweaker;
        public float accuracyAdjustment;
        public float speedAdjustment;
        public float fireRateAdjustment;*/
    }
}
