using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class TerainEditor : EditorWindow
{
    #region Statics
    [MenuItem("Window/Terain Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        TerainEditor thisWindow = EditorWindow.GetWindow(typeof(TerainEditor)) as TerainEditor;
        thisWindow.Show();
        thisWindow.minSize = new Vector2(1280, 720);
        thisWindow.Init();
    }
    #endregion
    #region local
    Texture2D backgroundTexture = new Texture2D(1,1);
    Vector2 viewPosition = Vector2.zero;
    Color seeThrough = new Color(0, 0, 0, 0);
    Dictionary<Vector3,char> currentLevel;
    float viewAspect { get { return 1f / zoomSize; } }

    //editor defined varibles
    float sideBarWidth = 250f;
    int sizeX = 60, sizeY = 60;
    int zoomSize = 10 ;
    // Update is called once per frame
    void OnGUI()
    {
        if (drawFieldSize(1))
            UpdateBackgroundTexture();

        if (GUI.Button(new Rect(getSideBarX(), 45, sideBarWidth, 15), "Reset"))
        {
            currentLevel = new Dictionary<Vector3, char>();
            viewPosition = new Vector2();
            UpdateBackgroundTexture();
        }

        if (Event.current.type == (EventType.MouseDrag | EventType.MouseDown) && Event.current.button == 0) 
        {
            addNewVoxel(Mathf.FloorToInt((Event.current.mousePosition.x * viewAspect) - viewPosition.x ), 0, Mathf.FloorToInt(((position.height - Event.current.mousePosition.y)* viewAspect) + viewPosition.y) , '0');
            UpdateBackgroundTexture();
            //Debug.Log(new Vector3(Mathf.FloorToInt((Event.current.mousePosition.x * viewAspect)), 0, Mathf.FloorToInt((position.height - Event.current.mousePosition.y) * viewAspect)));
        }

       /* if (Event.current.type == EventType.MouseDrag && Event.current.button ==1)
        {
            viewPosition += Event.current.delta * viewAspect;
            UpdateBackgroundTexture();
        }

        if(Event.current.type == EventType.ScrollWheel)
        {
            if (Event.current.delta.y > 0 && zoomSize < 30)
                zoomSize++;
            if (Event.current.delta.y < 0 && zoomSize > 1)
                zoomSize--;

            UpdateBackgroundTexture();
        }*/
        EditorGUI.DrawTextureTransparent(new Rect(0, 0, getSideBarX(), position.height), backgroundTexture);
    }

    #region internalFunctions
    void UpdateBackgroundTexture()
    {
        backgroundTexture = new Texture2D(Mathf.FloorToInt(getSideBarX() * viewAspect), Mathf.FloorToInt(position.height * viewAspect), TextureFormat.ARGB32, false);
        backgroundTexture.filterMode = FilterMode.Point;
        backgroundTexture.anisoLevel = 0;
        for (int x = 0; x < backgroundTexture.width; x++)
        {
            for (int y = 0; y < backgroundTexture.height; y++)
            {
                if (Inview(x - (int)viewPosition.x, y - (int)viewPosition.y))
                {
                    backgroundTexture.SetPixel(x, y, getVoxelColor(x - (int)viewPosition.x, 0, y + (int)viewPosition.y));
                }
              //  else
               // {
               //     backgroundTexture.SetPixel(x - (int)viewPosition.x, y + (int)viewPosition.y, Color.black);
               // }
            }
        }
        backgroundTexture.Apply();
        Repaint();
    }

    public void Init()
    {
        UpdateBackgroundTexture();
    }

    /// <summary>
    /// needs to bechanged
    /// </summary>
    /// <returns></returns>
    bool mouseInMiddleWindow()
    {
        Vector2 mousepos = Input.mousePosition;
        if (mousepos.x < position.xMax && mousepos.x > position.xMin)
        {
            if (mousepos.y < position.yMax && mousepos.y > position.yMin)
            {
                return true;
            }
        }
        return false;
    }

    bool CanMoveView()
    {
        
        return false;
    }

    bool Inview(int x, int z)
    {
        
        if (x > viewPosition.x + sizeX  || x < viewPosition.x)
            return false;
        if (z > viewPosition.y + sizeY  || z < viewPosition.y)
            return false;
        return true;
    }

    float getSideBarX()
    {
        return position.width - sideBarWidth;
    }

    void addNewVoxel(int x, int y, int z,char c)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();
        if (!Inview(x, z))
            return;

        Vector3 v = new Vector3(x, y, z);

        if (!currentLevel.ContainsKey(v))
        {
            currentLevel.Add(v, c);
        }
        else if (currentLevel.ContainsKey(v))
        {
            currentLevel[v] = c;
        }
    }

    //Size is in voxels
    bool drawFieldSize(float y)
    {
        Vector2 s = new Vector2(sizeX, sizeY);
        s = EditorGUI.Vector2Field(new Rect(getSideBarX(), y, sideBarWidth, 20), "Size", s);

        if (s.y != sizeY || s.x != sizeX)
        {
            if (s.y > 1)
                sizeY = (int)s.y;
            else
                sizeY = 1;
            if (s.x > 1)
                sizeX = (int)s.x;
            else
                sizeX = 1;
            return true;
        }
        return false;
    }

    Color getVoxelColor(int x, int y, int z)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();

        Vector3 v = new Vector3(x, y, z);
        
        if (currentLevel.ContainsKey(v))
        {
            switch (currentLevel[v])
            {
                case '0':
                    return Color.cyan;
                case '1':
                    return Color.green;
                default:
                    return Color.gray;
            }
        }
        return Color.gray;
    }
    #endregion
    #endregion
}
