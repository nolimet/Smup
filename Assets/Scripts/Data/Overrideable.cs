using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [Serializable] [InlineProperty]
    public class Overrideable<T> where T : IEquatable<T>, IFormattable
    {
        [LabelText("Value")]
        [HorizontalGroup]
        [ShowInInspector]
        [PropertyOrder(1)]
        public T Value => _hasOverride ? _overrideValue : defaultValue;

        [LabelText("Default")]
        [HorizontalGroup]
        [SerializeField] private T defaultValue;

        private T _overrideValue;
        private bool _hasOverride;

        public Overrideable(T value) => defaultValue = value;

        public void SetOverride(T newOverride)
        {
            _hasOverride = true;
            _overrideValue = newOverride;
        }

        public void ClearOverride()
        {
            _hasOverride = false;
            _overrideValue = default;
        }

        public static implicit operator Overrideable<T>(T value) => new(value);

        public static implicit operator T(Overrideable<T> value) => value.Value;
    }
}
