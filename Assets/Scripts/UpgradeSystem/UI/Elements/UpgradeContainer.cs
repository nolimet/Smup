using UnityEngine.UIElements;

namespace UpgradeSystem.UI.Elements
{
	[UxmlElement]
	public partial class UpgradeContainer : VisualElement
	{
		public const string ussClassName = "upgrade-container";

		public UpgradeContainer()
		{
			AddToClassList(ussClassName);
		}

		private void GenerateElements() { }
	}
}
