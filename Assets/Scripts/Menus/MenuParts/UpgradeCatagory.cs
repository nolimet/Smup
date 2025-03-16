using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Menus.MenuParts
{
    public class UpgradeCatagory : MonoBehaviour
    {
        [FormerlySerializedAs("Name")] [SerializeField] private Text displayName;
        [FormerlySerializedAs("Discription")] [SerializeField] private Text description;
        [FormerlySerializedAs("ButtonObj")] [SerializeField] private Button expandButton;

        public string DisplayName
        {
            set => displayName.text = value;
        }

        public string Description
        {
            set => description.text = value;
        }

        public Button ExpandButton => expandButton;

        public void SetText(string displayNameText, string descrption)
        {
            displayName.text = displayNameText;
            description.text = descrption;
        }
    }
}
