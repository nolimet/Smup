using UnityEngine;
using UnityEditor;
using Util;
using Util.Serial;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class TerainEditor : EditorWindow
{
    #region Statics
    [MenuItem("GameEdit/Formation Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        TerainEditor thisWindow = GetWindow(typeof(TerainEditor)) as TerainEditor;
        thisWindow.Show();
        thisWindow.minSize = new Vector2(1280, 720);
        thisWindow.Init();
    }
    #endregion
    #region local
    Texture2D backgroundTexture = null;
    Vector2 viewPosition = Vector2.zero;
//    Color seeThrough = new Color(0, 0, 0, 0);
    Dictionary<Vector3,char> currentLevel;
    EnemyStats.Type selectedType = EnemyStats.Type.shooter;
    float viewAspect { get { return 1f / zoomSize; } }

    //editor defined varibles
    float sideBarWidth = 250f;
    int sizeX = 30, sizeY = 20, zIndex = 0;
    int zoomSize = 15 ;
    string WaveName;
    // Update is called once per frame
    void OnGUI()
    {
        //calculate background
        if (drawFieldSize(1))
            UpdateBackgroundTexture();

        //set enemy type
        selectedType = (EnemyStats.Type)EditorGUI.EnumPopup(new Rect(getSideBarX(), 65, sideBarWidth, 15), "Selected Enemy", selectedType);

        #region save, load and reset
        //clear screen
        if (GUI.Button(new Rect(getSideBarX(), 85, sideBarWidth, 15), "Reset"))
        {
            currentLevel = new Dictionary<Vector3, char>();
            viewPosition = new Vector2();
            UpdateBackgroundTexture();
        }

        //save DAta
        if (GUI.Button(new Rect(getSideBarX(), 105, sideBarWidth, 15), "SAVE WAVE"))
            Save();
        //load Data
        if (GUI.Button(new Rect(getSideBarX(), 125, sideBarWidth, 15), "LOAD WAVE"))
            Load();
        #endregion

        #region Controles
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 || Event.current.type == EventType.MouseDown && Event.current.button == 0) 
        {
            addNewVoxel(Mathf.FloorToInt((Event.current.mousePosition.x * viewAspect) - viewPosition.x ), Mathf.FloorToInt(((position.height - Event.current.mousePosition.y)* viewAspect) + viewPosition.y), zIndex, selectedType);
            UpdateBackgroundTexture();
        }
        if (Event.current.type == EventType.MouseDrag && Event.current.button == 1 || Event.current.type == EventType.MouseDown && Event.current.button == 1)
        {
            RemoveVoxel(Mathf.FloorToInt((Event.current.mousePosition.x * viewAspect) - viewPosition.x), Mathf.FloorToInt(((position.height - Event.current.mousePosition.y) * viewAspect) + viewPosition.y), zIndex);
            UpdateBackgroundTexture();
        }
            if (backgroundTexture == null)
        {
            UpdateBackgroundTexture();
        }
        #endregion

        //draws backgound texture
        EditorGUI.DrawTextureTransparent(new Rect(0, 0, getSideBarX(), position.height), backgroundTexture);
    }

    #region internalFunctions
    void UpdateBackgroundTexture()
    {
        if (backgroundTexture == null)
        {
            backgroundTexture = new Texture2D(Mathf.FloorToInt(getSideBarX() * viewAspect), Mathf.FloorToInt(position.height * viewAspect), TextureFormat.ARGB32, false);
            backgroundTexture.filterMode = FilterMode.Point;
            backgroundTexture.anisoLevel = 0;
        }
        else
            backgroundTexture.Resize(Mathf.FloorToInt(getSideBarX() * viewAspect), Mathf.FloorToInt(position.height * viewAspect));
        
        for (int x = 0; x < backgroundTexture.width; x++)
        {
            for (int y = 0; y < backgroundTexture.height; y++)
            {
                if (Inview(x - (int)viewPosition.x, y - (int)viewPosition.y))
                {
                    backgroundTexture.SetPixel(x, y, getVoxelColor(x - (int)viewPosition.x, y + (int)viewPosition.y, zIndex));
                }
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

    void addNewVoxel(int x, int y, int z,EnemyStats.Type Type)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();
        if (!Inview(x, z))
            return;

        char c = System.Convert.ToChar((int)Type);

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

    void RemoveVoxel(int x, int y, int z)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();
        if (!Inview(x, z))
            return;

        Vector3 v = new Vector3(x, y, z);
        if (currentLevel.ContainsKey(v))
        {
            currentLevel.Remove(v);
        }
    }

    //Size is in voxels
    bool drawFieldSize(float y)
    {
        Vector2 s = new Vector2(sizeX, sizeY);
        int z = zIndex;
        s = EditorGUI.Vector2Field(new Rect(getSideBarX(), y, sideBarWidth, 20), "Size", s);
        z = EditorGUI.IntField(new Rect(getSideBarX(), 45, sideBarWidth, 15), "zIndex", z);

        if (s.y != sizeY || s.x != sizeX || z != zIndex)
        {
            if (s.y > 1)
                sizeY = (int)s.y;
            else
                sizeY = 1;
            if (s.x > 1)
                sizeX = (int)s.x;
            else
                sizeX = 1;
            if (z > 0)
                zIndex = z;
            else
                zIndex = 0;
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
                case 'Y':
                    return Color.cyan;
                case 'Z':
                    return Color.green;
                case '[':
                    return Color.red;
                case ' ':
                    return Color.gray;
                default:
                    return Color.gray;
            }
        }
        return Color.gray;
    }

    void Save()
    {
        int hz = 0;
        foreach(Vector3 v in currentLevel.Keys)
        {
            if (v.z > hz)
                hz = (int)v.z;
        }
        Debug.Log(hz);
        hz++;

        float h = -1;
        foreach (KeyValuePair<Vector3, char> k in currentLevel)
            if (k.Key.z > h)
                h = k.Key.z;

        WaveClass SaveData = new WaveClass(currentLevel, new Vector3(sizeX, sizeY, (int)h + 1));

        Serialization.Save("Wave1", Serialization.fileTypes.wave, SaveData);
    }

    void Load()
    {
        WaveClass loadedData = new WaveClass();
        
        if (!Serialization.Load("Wave1", Serialization.fileTypes.wave, ref loadedData))
        {
            currentLevel = new Dictionary<Vector3, char>();
            UpdateBackgroundTexture();
            return;
        }

        Debug.Log("Wave points: " + loadedData.waves.Length);
        currentLevel = loadedData.Convert();
        UpdateBackgroundTexture();
    }
    #endregion
    #endregion
}
