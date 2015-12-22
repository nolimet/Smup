using UnityEngine;
using System.Collections;

public class GridlayoutWidthSetter : MonoBehaviour {
    RectTransform rt;
    UnityEngine.UI.GridLayoutGroup g;
    public int ChildrenNeededToScroll = 8;
    int childrenLast;

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
            CustomGrid c = GetComponent<CustomGrid>();
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
        if (childrenLast != rt.childCount)
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
    }
}