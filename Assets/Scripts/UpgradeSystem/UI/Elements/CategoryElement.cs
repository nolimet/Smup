using UnityEngine.UIElements;

namespace UpgradeSystem.UI.Elements
{
	public class CategoryElement : VisualElement
	{
		public const string ussClassName = "category";

		public CategoryElement()
		{
			AddToClassList(ussClassName);
		}
	}
}
