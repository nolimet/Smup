using UnityEngine;
using System.Collections;
namespace Util.UI
{
    public class GridLayoutHeightSetter : MonoBehaviour
    {
        public bool onlyUseActive;
        float o;
        // Use this for initialization
        void Start()
        {
            o = ((RectTransform)transform).sizeDelta.y;
        }

        void Update()
        {
            if (GetComponent<UnityEngine.UI.GridLayoutGroup>() && GetComponent<UnityEngine.UI.GridLayoutGroup>().preferredHeight > o)
                ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, transform.childCount * (GetComponent<UnityEngine.UI.GridLayoutGroup>().cellSize.y + GetComponent<UnityEngine.UI.GridLayoutGroup>().spacing.y));
            if(GetComponent <CustomGrid>())
            {
                CustomGrid c = GetComponent<CustomGrid>();
                ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, c.transform.childCount * ( c.ObjSize.y));
                //((RectTransform)transform).
            }
        }
    }
}