using System;

namespace UpgradeSystem.Attributes
{
    public class CategoryAttribute : Attribute
    {
        public readonly string Name;
        public CategoryAttribute(string name) => Name = name;
    }
}
