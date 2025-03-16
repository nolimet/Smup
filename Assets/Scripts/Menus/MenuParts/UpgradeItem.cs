using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Menus.MenuParts
{
    public class UpgradeItem : MonoBehaviour
    {
        [FormerlySerializedAs("Name")] [SerializeField] private Text displayName;
        [FormerlySerializedAs("Discription")] [SerializeField] private Text description;
        [FormerlySerializedAs("Price")] [SerializeField] private Text price;
        [FormerlySerializedAs("CurrentLevel")] [SerializeField] private Text currentLevel;
        [FormerlySerializedAs("Buy")] [SerializeField] private Button buyButton;

        public string DisplayName
        {
            set => displayName.text = value;
        }

        public string Description
        {
            set => description.text = value;
        }

        public Button BuyButton => buyButton;

        public string Price
        {
            set => price.text = value;
        }

        public string CurrentLevel
        {
            get => currentLevel.text;
            set => currentLevel.text = value;
        }
    }
}
