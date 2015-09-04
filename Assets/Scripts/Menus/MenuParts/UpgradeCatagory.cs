using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeCatagory : MonoBehaviour
{
    public Text Name;
    public Text Discription;
    public Button ButtonObj;

    public void SetText(string Name, string Discription)
    {
        this.Name.text = Name;
        this.Discription.text = Discription;
    }
}
