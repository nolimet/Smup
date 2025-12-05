using UnityEngine.UIElements;
using UpgradeSystem.Attributes;

namespace UpgradeSystem.UI.Elements
{
    public class CategoryElement : Foldout
    {
        public new const string ussClassName = "category";

        public CategoryElement(CategoryAttribute categoryAttr)
        {
            AddToClassList(ussClassName);
            text = categoryAttr.Name;
        }
    }
}
