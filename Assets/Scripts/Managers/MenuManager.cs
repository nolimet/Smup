using UnityEngine;
using System.Collections;

public class MenuManager : MonoBehaviour {

    public GameObject[] SubMenus;
    
    void Start()
    {
        CloseSubMenu();
    }

    public void OpenMenu(int index)
    {
        CloseSubMenu();

        if (index < SubMenus.Length)
        {
            SubMenus[index].SetActive(true);
        }
    }

    public void CloseSubMenu()
    {
        foreach (GameObject item in SubMenus)
        {
            item.SetActive(false);
        }
    }
}
