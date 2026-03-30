using System;

namespace Smup.UpgradeSystem.Attributes
{
    public class CategoryAttribute : Attribute
    {
        public readonly string Name;
        public CategoryAttribute(string name) => Name = name;
    }
}
