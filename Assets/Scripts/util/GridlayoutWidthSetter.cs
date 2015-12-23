using UnityEngine;
using System.Collections;

public class GridlayoutWidthSetter : MonoBehaviour {
    RectTransform rt;
    UnityEngine.UI.GridLayoutGroup g;
    CustomGrid c;
    public int ChildrenNeededToScroll = 8;
    public bool onlySetContainerHeight = true;
    int childrenLast;
    int chilCount;
    int AcLast = 0;
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
        if (rt.childCount > ChildrenNeededToScroll)
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * rt.childCount);
        }
        else
        {
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * ChildrenNeededToScroll);
        }
    }
   
    void Update()
    {
        if (childrenLast != rt.childCount || activeChildCount() != AcLast)
        {
            if (rt.childCount > ChildrenNeededToScroll || activeChildCount() > ChildrenNeededToScroll)
            {
                if (!c)
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * rt.childCount);
                else
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * Mathf.FloorToInt(rt.childCount / c.maxRows));
            }
            else
            {
                if (!c)
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * ChildrenNeededToScroll);
                else
                    rt.sizeDelta = new Vector2(rt.sizeDelta.x, (cellSize.y + spacing.y) * (ChildrenNeededToScroll / c.maxRows));
            }

            AcLast = activeChildCount();
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