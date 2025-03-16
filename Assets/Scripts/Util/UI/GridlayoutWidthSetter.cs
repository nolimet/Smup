using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Util.UI
{
    public class GridlayoutWidthSetter : MonoBehaviour
    {
        private RectTransform _rt;
        private GridLayoutGroup _g;
        private CustomGrid _c;
        [FormerlySerializedAs("ChildrenNeededToScroll")] public int childrenNeededToScroll = 8;
        public bool onlySetContainerHeight = true;
        public bool onlyUseActiveChildren = true;
        private int _childrenLast = 0;
        private int _childrenCount;
        private Vector2 _spacing, _cellSize;

        private void Awake()
        {
            _rt = (RectTransform)transform;

            if (GetComponent<GridLayoutGroup>())
            {
                _g = GetComponent<GridLayoutGroup>();
                _g.cellSize = new Vector2(_rt.rect.width, _g.cellSize.y);
                _spacing = _g.spacing;
                _cellSize = _g.cellSize;
            }

            if (GetComponent<CustomGrid>())
            {
                _c = GetComponent<CustomGrid>();
                if (!onlySetContainerHeight)
                    _c.objSize = new Vector2(_rt.rect.width, _c.objSize.y);
                _cellSize = _c.objSize;
                _spacing = _c.maxSpacing;
            }
        }

        public void ForceUpdate()
        {
            _childrenCount = ActiveChildCount();
            if (_childrenCount > childrenNeededToScroll)
            {
                if (!_c)
                    _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, (_cellSize.y + _spacing.y) * _childrenCount);
                else
                    _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, (_c.objSize.y + (_c.padding.y + _c.currentSpacing.y)) * Mathf.CeilToInt(_childrenCount / (float)_c.maxRows));
            }
            else
            {
                if (!_c)
                    _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, (_cellSize.y + _spacing.y) * childrenNeededToScroll);
                else
                    _rt.sizeDelta = new Vector2(_rt.sizeDelta.x, (_c.objSize.y + (_c.padding.y + _c.currentSpacing.y)) * (childrenNeededToScroll / _c.maxRows));
            }
        }

        private void Update()
        {
            _childrenCount = ActiveChildCount();
            if (_childrenLast != _childrenCount) ForceUpdate();
        }

        private int ActiveChildCount()
        {
            var r = 0;
            foreach (Transform t in transform)
                if (t.gameObject.activeSelf)
                    r++;
            return r;
        }
    }
}
