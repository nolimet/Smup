using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpgradeItem : MonoBehaviour
{
    public Text Name, Discription, Price, CurrentLevel;
    public Button Buy;

    public void OnEnable()
    {
        RectTransform r = (RectTransform)transform;
        r.anchoredPosition = new Vector2(0, -400f);
    }
}
