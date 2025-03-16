using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class MenuManager : MonoBehaviour
    {
        [FormerlySerializedAs("SubMenus")] [SerializeField] private GameObject[] subMenus;

        private void Start()
        {
            CloseSubMenu();
        }

        public void OpenMenu(int index)
        {
            CloseSubMenu();

            if (index < subMenus.Length) subMenus[index].SetActive(true);
        }

        public void CloseSubMenu()
        {
            foreach (var item in subMenus) item.SetActive(false);
        }
    }
}
