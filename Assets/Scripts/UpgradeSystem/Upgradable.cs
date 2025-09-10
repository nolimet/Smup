using System;
using Sirenix.OdinInspector;

namespace UpgradeSystem
{
    [Serializable]
    public class Upgradable<TValueType>
    {
        public const int ByteSize = 4;

        public delegate TValueType GetValueDelegate(int level);

        public delegate double GetCostDelegate(int level);

        [ShowInInspector] private int _currentLevel;
        [ShowInInspector] [ReadOnly] private readonly int _maxLevel;

        private readonly GetValueDelegate _getValueFunc;
        [ShowInInspector] [ReadOnly] public TValueType Value => _getValueFunc(_currentLevel);

        private readonly GetCostDelegate _getCostFunc;
        public double Cost => _getCostFunc(_currentLevel);

        public Upgradable(int maxLevel, GetValueDelegate getValueFunc, GetCostDelegate getCostFunc)
        {
            _maxLevel = maxLevel;
            _getValueFunc = getValueFunc;
            _getCostFunc = getCostFunc;
        }

        public byte[] ToBytes() => BitConverter.GetBytes(_currentLevel);

        public void ApplyBytes(ReadOnlySpan<byte> bytes)
        {
            _currentLevel = BitConverter.ToInt32(bytes);
        }

        public static implicit operator TValueType(Upgradable<TValueType> value) => value.Value;
    }
}
