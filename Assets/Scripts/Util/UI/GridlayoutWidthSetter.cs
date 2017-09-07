using UnityEngine;
using System.Collections;
namespace Util.UI
{
    public class GridlayoutWidthSetter : MonoBehaviour
    {
        RectTransform rt;
        UnityEngine.UI.GridLayoutGroup g;
        CustomGrid c;
        public int ChildrenNeededToScroll = 8;
        public bool onlySetContainerHeight = true;
        public bool onlyUseActiveChildren = true;
        int childrenLast = 0;
        int ChildrenCount = 0;
        Vector2 spacing, cellSize;

        void Awake()
        {
            rt = (RectTransform)transform;

            if (GetComponent<UnityEngine.UI.GridLayoutGroup>())
            {
                g = GetComponent<UnityEngine.UI.GridLayoutGroup>();
                g.cellSize = new Vector2(rt.rect.width, g.cellSize.y);
                spacing = g.spacing;
                cellSize = g.cellSize;
            }

            if (GetComponent<CustomGrid>())
            {
                c = GetComponent<CustomGrid>();
                if (!onlySetContainerHeight)
                    c.ObjSize = new Vector2(rt.rect.width, c.ObjSize.y);
                cellSize = c.ObjSize;
                spacing = c.maxSpacing;

            }
        }

        public void ForceUpdate()
        {
            ChildrenCount = activeChildCount();
            if (ChildrenCount > ChildrenNeededToScroll)
            {
                if (!c)
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * ChildrenCount);
                else
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (c.ObjSize.y + (c.padding.y + c.CurrentSpacing.y)) * Mathf.CeilToInt(ChildrenCount / (float)c.maxRows));
            }
            else
            {
                if (!c)
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * ChildrenNeededToScroll);
                else
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (c.ObjSize.y + (c.padding.y + c.CurrentSpacing.y)) * (ChildrenNeededToScroll / c.maxRows));
            }
        }

        void Update()
        {
            ChildrenCount = activeChildCount();
            if (childrenLast != ChildrenCount)
            {

                ForceUpdate();
            }
        }

        int activeChildCount()
        {
            int r = 0;
            foreach (Transform t in transform)
                if (t.gameObject.activeSelf)
                    r++;
            return r;
        }
    }
}