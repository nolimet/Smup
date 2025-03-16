using System;

namespace Util.DebugHelpers
{
    [Serializable]
    public class ValueWrapper<T>
    {
        public T Value { get; set; }
    }
}
