using System;
using Sirenix.OdinInspector;

namespace UpgradeSystem
{
    [Serializable]
    public abstract class Upgradable
    {
        public const int ByteSize = 4;
        public static readonly byte[] Empty = new byte[ByteSize];

        public delegate double GetCostDelegate(int level);

        private readonly GetCostDelegate _getCostFunc;
        public double Cost => _getCostFunc(CurrentLevel);

        [ShowInInspector] protected int CurrentLevel;
        [ShowInInspector] [ReadOnly] protected readonly int MaxLevel;

        protected Upgradable(int maxLevel, GetCostDelegate getCostFunc)
        {
            _getCostFunc = getCostFunc;
            MaxLevel = maxLevel;
        }

        public byte[] ToBytes() => BitConverter.GetBytes(CurrentLevel);

        public void ApplyBytes(ReadOnlySpan<byte> bytes)
        {
            CurrentLevel = BitConverter.ToInt32(bytes);
        }
    }

    [Serializable]
    public class Upgradable<TValueType> : Upgradable
    {
        public delegate TValueType GetValueDelegate(int level);

        private readonly GetValueDelegate _getValueFunc;
        [ShowInInspector] [ReadOnly] public TValueType Value => _getValueFunc(CurrentLevel);

        public Upgradable(int maxLevel, GetValueDelegate getValueFunc, GetCostDelegate getCostFunc) : base(maxLevel, getCostFunc) => _getValueFunc = getValueFunc;

        public static implicit operator TValueType(Upgradable<TValueType> value) => value.Value;
    }
}
