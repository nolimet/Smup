using UnityEngine;
using UnityEngine.UI;

namespace Util.UI
{
    public class GridLayoutHeightSetter : MonoBehaviour
    {
        public bool onlyUseActive;

        private float _o;

        // Use this for initialization
        private void Start()
        {
            _o = ((RectTransform)transform).sizeDelta.y;
        }

        private void Update()
        {
            if (GetComponent<GridLayoutGroup>() && GetComponent<GridLayoutGroup>().preferredHeight > _o)
                ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, transform.childCount * (GetComponent<GridLayoutGroup>().cellSize.y + GetComponent<GridLayoutGroup>().spacing.y));
            if (GetComponent<CustomGrid>())
            {
                var c = GetComponent<CustomGrid>();
                ((RectTransform)transform).sizeDelta = new Vector2(((RectTransform)transform).sizeDelta.x, c.transform.childCount * c.objSize.y);
                //((RectTransform)transform).
            }
        }
    }
}
