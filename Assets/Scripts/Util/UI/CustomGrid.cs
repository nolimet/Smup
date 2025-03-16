using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Util.UI
{
    /// <summary>
    ///  A grid sorting method that has the abilty create simple soothend animations.
    ///  It also only does things when it detects changes making it quite light;
    /// </summary>
    public class CustomGrid : MonoBehaviour
    {
        /// <summary>
        /// Size that each object will have
        /// </summary>
        [FormerlySerializedAs("ObjSize")] public Vector2 objSize;

        /// <summary>
        /// forced space between each object
        /// </summary>
        public Vector2 padding;

        /// <summary>
        /// the maxium space between two objects
        /// </summary>
        public Vector2 maxSpacing;

        /// <summary>
        /// The anchor point or from wich the object wil be centered
        /// </summary>
        [FormerlySerializedAs("AnchorPoint")] public Vector2 anchorPoint = Vector2.zero;

        /// <summary>
        /// The current space between the objects
        /// </summary>
        [FormerlySerializedAs("CurrentSpacing")] [ReadOnly] public Vector2 currentSpacing = Vector2.zero;

        /// <summary>
        /// The maxium number of rows
        /// </summary>
        [Range(1, 500)] //can ve changed to anynumber above 0
        public int maxRows = 2;

        /// <summary>
        /// The minum objecs on a single row needed before it creates a second row
        /// </summary>
        [Range(1, 500)] public int minNeededForFirstRow = 2;

        /// <summary>
        /// The y offset used for when the objects are not well centered them selfs
        /// </summary>
        [Range(0, 1f)] public float yoffset;

        /// <summary>
        /// Does the grid update continuesly. usefull when debugging the grid or seeing if everything works correctly.
        /// Recommened to have it turned of when you are building the game because it saves preformance
        /// </summary>
        public bool continuesUpdates;

        /// <summary>
        /// Enable the animations so that the elements move towards the new points instead of teleporting
        /// </summary>
        public bool useAnimations;

        /// <summary>
        /// used for the update tigger. When the number of childeren changes
        /// </summary>
        private int _childCountLast;

        /// <summary>
        /// Are they at the objects at there new positions
        /// </summary>
        private bool _atNewPos;

        /// <summary>
        /// Used for the animations so they run smoothly
        /// </summary>
        private float _startTime;

        /// <summary>
        /// List that contains positional data for the objects
        /// </summary>
        private List<Vector2> _newPos = new(), _startPos = new();

        /// <summary>
        /// List of active game objects
        /// </summary>
        [FormerlySerializedAs("ActiveChildren")] [ReadOnly] [SerializeField] private List<RectTransform> activeChildren;

        /// <summary>
        /// Toggle to only use enabled game ojbects
        /// </summary>
        [SerializeField] private bool onlyUseActiveObjects;

        private RectTransform _t;

        private void Start()
        {
            _t = (RectTransform)transform;
            _atNewPos = false;
            activeChildren = new List<RectTransform>();
        }

        private void OnEnable()
        {
            SetChildPosition();
            SetChildSize();
            SnapToPos();
            _atNewPos = false;
        }

        public void ForceUpdate()
        {
            OnEnable();
        }

        private void Awake()
        {
            _atNewPos = false;
        }

        private void Update()
        {
            if (_childCountLast != GetChildCount())
            {
                SetChildSize();
                SetChildPosition();
                _childCountLast = GetChildCount();
            }
            else if (_atNewPos && continuesUpdates)
            {
                SetChildSize();
                SetChildPosition();
                SnapToPos();
            }

            if (!_atNewPos)
            {
                if ((Time.time - _startTime) * 4f <= 1f && useAnimations)
                {
                    MoveObjectsToPos();
                }
                else
                {
                    _atNewPos = true;
                    SnapToPos();
                }
            }
        }

        private void SetChildSize()
        {
            RectTransform child;
            for (var i = 0; i < transform.childCount; i++)
            {
                child = (RectTransform)_t.GetChild(i);
                child.sizeDelta = objSize;
                // child.name = "Obj " + i.ToString();
                //child.anchorMax = Vector2.one / 2;
                //child.anchorMin = Vector2.one / 2;
                child.anchorMax = anchorPoint;
                child.anchorMin = anchorPoint;
            }
        }

        private void SetChildPosition()
        {
            _startTime = Time.time;
            _atNewPos = false;
            _startPos.Clear();
            _newPos.Clear();
            activeChildren.Clear();

            if (!_t)
                _t = (RectTransform)transform;

            var wh = _t.rect;
            var noOfChilds = GetChildCount();
            float spacingY, spacingX;

            ///calculate HorizontalSpace
            if (noOfChilds == 1)
                spacingY = 0;
            else if (noOfChilds > minNeededForFirstRow) //diffrence less than 3 and 3 is special alignment
                spacingY = wh.height - Mathf.CeilToInt(noOfChilds / (float)maxRows) * objSize.y;
            else
                spacingY = wh.height - objSize.y * noOfChilds;

            if (noOfChilds < minNeededForFirstRow)
                spacingX = 0;
            else
                spacingX = wh.width - maxRows * objSize.x;

            spacingX /= maxRows;
            spacingY /= Mathf.CeilToInt(noOfChilds / (float)maxRows);

            if (spacingX < 0)
                spacingX = 0;
            if (spacingY < 0)
                spacingY = 0;
            if (spacingY > maxSpacing.y)
                spacingY = maxSpacing.y;
            if (spacingX > maxSpacing.x)
                spacingX = maxSpacing.x;

            currentSpacing = new Vector2(spacingX, spacingY);
            spacingX += padding.x;
            spacingY += padding.y;

            RectTransform child;
            float indexLocal = 0, origenX = 0;
            var z = spacingX + objSize.x;
            var row = 0;
            var totalCollums = Mathf.CeilToInt(noOfChilds / (float)maxRows);
            var rowSize = 0;
            var pos = Vector2.zero;

            for (var i = 0; i < noOfChilds; i++)
            {
                child = activeChildren[i];

                if (!onlyUseActiveObjects || (onlyUseActiveObjects && child.gameObject.activeSelf))
                {
                    if (i % maxRows == 0)
                    {
                        if ((i + 1) / maxRows < totalCollums - 1)
                            row = maxRows;
                        else
                            rowSize = row = noOfChilds - i;
                    }

                    if (noOfChilds <= minNeededForFirstRow)
                    {
                        indexLocal = noOfChilds / 2 - i;
                        if (noOfChilds == 2)
                            indexLocal -= 0.5f;
                        if (noOfChilds == 0)
                            indexLocal = 0;

                        pos = new Vector2(0f, indexLocal * (spacingY + objSize.y));
                    }
                    else
                    {
                        if (anchorPoint.y < 1)
                            indexLocal = noOfChilds / maxRows / 2f - i / maxRows;
                        else
                            indexLocal = -i / maxRows;
                        // Debug.Log(indexLocal);
                        if (noOfChilds % maxRows == 0)
                            indexLocal -= yoffset;

                        pos = new Vector2(0f, indexLocal * (spacingY + objSize.y));

                        if (maxRows > 1)
                        {
                            if ((i + 1) / maxRows < totalCollums - 1)
                                origenX = z * (maxRows + 1) / 2f;
                            else if (row == rowSize) origenX = z * (rowSize + 1) / 2f;

                            pos.x = -z * row + origenX;
                        }
                        else
                        {
                            pos.x = 0;
                        }
                    }

                    pos.x = Mathf.Ceil(pos.x);
                    pos.y = Mathf.Ceil(pos.y);

                    _startPos.Add(child.anchoredPosition);
                    _newPos.Add(pos);

                    row--;
                }
            }
        }

        private void MoveObjectsToPos()
        {
            RectTransform child;
            for (var i = 0; i < activeChildren.Count; i++)
            {
                child = activeChildren[i];
                child.anchoredPosition = Vector2.Lerp(_startPos[i], _newPos[i], (Time.time - _startTime) * 4f);
                RoundPos(child);
            }
        }

        private void RoundPos(RectTransform t)
        {
            var p = t.anchoredPosition;
            t.anchoredPosition = new Vector2(Mathf.Round(p.x), Mathf.Round(p.y));
        }

        private void SnapToPos()
        {
            RectTransform child;
            for (var i = 0; i < activeChildren.Count; i++)
            {
                child = activeChildren[i];
                child.anchoredPosition = _newPos[i];
            }
        }

        private int GetChildCount()
        {
            activeChildren.Clear();
            for (var i = 0; i < _t.childCount; i++)
                if (!onlyUseActiveObjects || (onlyUseActiveObjects && _t.GetChild(i).gameObject.activeSelf))
                    activeChildren.Add((RectTransform)_t.GetChild(i));

            if (onlyUseActiveObjects)
            {
                var r = 0;
                for (var i = 0; i < _t.childCount; i++)
                    if (_t.GetChild(i).gameObject.activeSelf)
                        r++;

                return r;
            }

            return _t.childCount;
        }
    }
}
