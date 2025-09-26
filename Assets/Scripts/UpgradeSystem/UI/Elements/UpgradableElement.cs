using UnityEngine.UIElements;

namespace UpgradeSystem.UI.Elements
{
	public class UpgradableElement : VisualElement
	{
		public const string ussClassName = "upgrade";

		public UpgradableElement()
		{
			AddToClassList(ussClassName);
		}
	}
}
