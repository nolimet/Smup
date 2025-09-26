using System;

namespace UpgradeSystem.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ElementAttribute : Attribute
    {
        public readonly string Name;
        public readonly string Description;

        public ElementAttribute(string name, string description = null)
        {
            Name = name;
            Description = description ?? string.Empty;
        }
    }
}
