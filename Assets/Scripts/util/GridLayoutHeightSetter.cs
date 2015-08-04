using UnityEngine;
using System.Collections;

public class GridLayoutHeightSetter : MonoBehaviour
{

    float o;
    // Use this for initialization
    void Start()
    {
        o = ((RectTransform)transform).sizeDelta.y;
    }
    
    void Update()
    {
        if (GetComponent<UnityEngine.UI.GridLayoutGroup>().preferredHeight > o)
            ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, GetComponent<UnityEngine.UI.GridLayoutGroup>().preferredHeight);
    }
}
