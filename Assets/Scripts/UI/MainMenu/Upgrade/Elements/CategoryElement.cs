using Smup.UpgradeSystem.Attributes;
using UnityEngine.UIElements;

namespace Smup.UI.MainMenu.Upgrade.Elements
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
