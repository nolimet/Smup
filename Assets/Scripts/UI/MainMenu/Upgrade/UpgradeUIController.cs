using UnityEngine;
using UnityEngine.UIElements;

namespace UpgradeSystem.UI
{
	[RequireComponent(typeof(UIDocument))]
	public class UpgradeUIController : MonoBehaviour
	{
		private UIDocument _document;

		private void Awake()
		{
			_document = GetComponent<UIDocument>();
		}
	}
}
