using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
        public Vector2 ObjSize;
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
        public Vector2 AnchorPoint = Vector2.zero;
        /// <summary>
        /// The current space between the objects
        /// </summary>
        [ReadOnly]
        public Vector2 CurrentSpacing = Vector2.zero;
        /// <summary>
        /// The maxium number of rows
        /// </summary>
        [Range(1, 500)] //can ve changed to anynumber above 0
        public int maxRows = 2;
        /// <summary>
        /// The minum objecs on a single row needed before it creates a second row
        /// </summary>
        [Range(1, 500)]
        public int minNeededForFirstRow = 2;
        /// <summary>
        /// The y offset used for when the objects are not well centered them selfs
        /// </summary>
        [Range(0, 1f)]
        public float yoffset;
        /// <summary>
        /// Does the grid update continuesly. usefull when debugging the grid or seeing if everything works correctly.
        /// Recommened to have it turned of when you are building the game because it saves preformance
        /// </summary>
        public bool continuesUpdates = false;
        /// <summary>
        /// Enable the animations so that the elements move towards the new points instead of teleporting
        /// </summary>
        public bool useAnimations = false;

        /// <summary>
        /// used for the update tigger. When the number of childeren changes
        /// </summary>
        int childCountLast = 0;

        /// <summary>
        /// Are they at the objects at there new positions
        /// </summary>
        bool atNewPos = false;
        /// <summary>
        /// Used for the animations so they run smoothly
        /// </summary>
        float StartTime;

        /// <summary>
        /// List that contains positional data for the objects
        /// </summary>
        List<Vector2> newPos = new List<Vector2>(), startPos = new List<Vector2>();
        /// <summary>
        /// List of active game objects
        /// </summary>
        [ReadOnly,SerializeField]
        List<RectTransform> ActiveChildren;
        /// <summary>
        /// Toggle to only use enabled game ojbects
        /// </summary>
        [SerializeField]
        bool onlyUseActiveObjects;

        RectTransform t;

        void Start()
        {
            t = (RectTransform)transform;
            atNewPos = false;
            ActiveChildren = new List<RectTransform>();
        }

        void OnEnable()
        {
            setChildPosition();
            setChildSize();
            SnapToPos();
            atNewPos = false;
        }

        public void ForceUpdate()
        {
            OnEnable();
        }

        void Awake()
        {
            atNewPos = false;
        }

        void Update()
        {
            if (childCountLast != getChildCount())
            {
                setChildSize();
                setChildPosition();
                childCountLast = getChildCount();
            }
            else if (atNewPos && continuesUpdates)
            {
                setChildSize();
                setChildPosition();
                SnapToPos();

            }
            if (!atNewPos)
            {
                if ((Time.time - StartTime) * 4f <= 1f && useAnimations)
                    MoveObjectsToPos();
                else
                {
                    atNewPos = true;
                    SnapToPos();
                }
            }
        }

        void setChildSize()
        {
            RectTransform child;
            for (int i = 0; i < transform.childCount; i++)
            {
                child = (RectTransform)t.GetChild(i);
                child.sizeDelta = ObjSize;
                // child.name = "Obj " + i.ToString();
                //child.anchorMax = Vector2.one / 2;
                //child.anchorMin = Vector2.one / 2;
                child.anchorMax = AnchorPoint;
                child.anchorMin = AnchorPoint;

            }
        }

        void setChildPosition()
        {
            StartTime = Time.time;
            atNewPos = false;
            startPos.Clear();
            newPos.Clear();
            ActiveChildren.Clear();

            if (!t)
                t = (RectTransform)transform;

            Rect WH = t.rect;
            int noOfChilds = getChildCount();
            float spacingY, spacingX;

            ///calculate HorizontalSpace
            if (noOfChilds == 1)
                spacingY = 0;
            else if (noOfChilds > minNeededForFirstRow) //diffrence less than 3 and 3 is special alignment
                spacingY = WH.height - ((Mathf.CeilToInt(noOfChilds / (float)maxRows)) * ObjSize.y);
            else
                spacingY = WH.height - (ObjSize.y * noOfChilds);

            if (noOfChilds < minNeededForFirstRow)
                spacingX = 0;
            else
                spacingX = WH.width - (maxRows * ObjSize.x);

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


            CurrentSpacing = new Vector2(spacingX, spacingY);
            spacingX += padding.x;
            spacingY += padding.y;


            RectTransform Child;
            float indexLocal = 0, OrigenX = 0;
            float z = (spacingX + ObjSize.x);
            int row = 0;
            int totalCollums = Mathf.CeilToInt(noOfChilds / (float)maxRows);
            int rowSize = 0;
            Vector2 pos = Vector2.zero;

            for (int i = 0; i < noOfChilds; i++)
            {
                Child = ActiveChildren[i];

                if (!onlyUseActiveObjects || onlyUseActiveObjects && Child.gameObject.activeSelf)
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

                        indexLocal = (noOfChilds / 2 - i);
                        if (noOfChilds == 2)
                            indexLocal -= 0.5f;
                        if (noOfChilds == 0)
                            indexLocal = 0;

                        pos = new Vector2(0f, indexLocal * (spacingY + ObjSize.y));
                    }
                    else
                    {
                        if (AnchorPoint.y < 1)
                        {
                            indexLocal = ((noOfChilds / maxRows) / 2f) - (i / maxRows);

                        }
                        else
                        {
                            indexLocal = (-i / maxRows);
                            // Debug.Log(indexLocal);
                        }

                        if (noOfChilds % maxRows == 0)
                            indexLocal -= yoffset;

                        pos = new Vector2(0f, indexLocal * (spacingY + ObjSize.y));

                        if (maxRows > 1)
                        {
                            if ((i + 1) / maxRows < totalCollums - 1)
                            {
                                OrigenX = (z * (maxRows + 1)) / 2f;
                            }
                            else if (row == rowSize)
                            {
                                OrigenX = (z * (rowSize + 1)) / 2f;
                            }

                            pos.x = (-z * row) + OrigenX;
                        }
                        else
                        {
                            pos.x = 0;
                        }
                    }
                    pos.x = Mathf.Ceil(pos.x);
                    pos.y = Mathf.Ceil(pos.y);

                    startPos.Add(Child.anchoredPosition);
                    newPos.Add(pos);

                    row--;
                }
            }
        }

        void MoveObjectsToPos()
        {
            RectTransform child;
            for (int i = 0; i < ActiveChildren.Count; i++)
            {
                child = ActiveChildren[i];
                child.anchoredPosition = Vector2.Lerp(startPos[i], newPos[i], (Time.time - StartTime) * 4f);
                RoundPos(child);
            }
        }

        void RoundPos(RectTransform t)
        {
            Vector2 p = t.anchoredPosition;
            t.anchoredPosition = new Vector2(Mathf.Round(p.x), Mathf.Round(p.y));
        }

        void SnapToPos()
        {
            RectTransform child;
            for (int i = 0; i < ActiveChildren.Count; i++)
            {
                child = ActiveChildren[i];
                child.anchoredPosition = newPos[i];
            }
        }

        int getChildCount()
        {
            ActiveChildren.Clear();
            for (int i = 0; i < t.childCount; i++)
            {
                if (!onlyUseActiveObjects || onlyUseActiveObjects && t.GetChild(i).gameObject.activeSelf)
                    ActiveChildren.Add((RectTransform)t.GetChild(i));
            }

            if (onlyUseActiveObjects)
            {
                int r = 0;
                for (int i = 0; i < t.childCount; i++)
                {
                    if (t.GetChild(i).gameObject.activeSelf)
                        r++;
                }

                return r;
            }
            return t.childCount;
        }
    }
}