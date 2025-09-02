using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace OverrideTest
{
    [Serializable] [InlineProperty]
    public class Overrideable<T> where T : IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        [LabelText("Value")]
        [HorizontalGroup]
        [ShowInInspector] 
        [PropertyOrder(1)]
        public T Value
        {
            get
            {
                if (_hasOverride)
                    return _overrideValue;
                return defaultValue;
            }
        }

        [LabelText("Default")]
        [HorizontalGroup]
        [SerializeField] private T defaultValue;
        private T _overrideValue;
        private bool _hasOverride = false;

        public Overrideable(T value)
        {
            defaultValue = value;
        }

        public void SetOverride(T newOverride)
        {
            _hasOverride = true;
            _overrideValue = newOverride;
        }

        public void ClearOverride()
        {
            _hasOverride = false;
            _overrideValue = default(T);
        }

        public static implicit operator Overrideable<T>(T value)
        {
            return new Overrideable<T>(value);
        }
    }

    public class Test
    {
        private Overrideable<int> intTest = 4;

        void TestThing()
        {
            intTest.SetOverride(1);
        }
    }
}
