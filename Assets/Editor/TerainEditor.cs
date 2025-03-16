using System;
using System.Collections.Generic;
using Enemies_old;
using UnityEditor;
using UnityEngine;
using Util.Saving;
using Vector3 = UnityEngine.Vector3;

public class TerainEditor : EditorWindow
{
    #region Statics

    [MenuItem("GameEdit/Formation Editor")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        var thisWindow = GetWindow(typeof(TerainEditor)) as TerainEditor;
        thisWindow.Show();
        thisWindow.minSize = new Vector2(1280, 720);
        thisWindow.Init();
    }

    #endregion

    #region local

    private Texture2D backgroundTexture = null;

    private Vector2 viewPosition = Vector2.zero;

//    Color seeThrough = new Color(0, 0, 0, 0);
    private Dictionary<Vector3, char> currentLevel;
    private EnemyStats.Type selectedType = EnemyStats.Type.Shooter;
    private float viewAspect => 1f / zoomSize;

    //editor defined varibles
    private float sideBarWidth = 250f;
    private int sizeX = 30, sizeY = 20, zIndex = 0;
    private int zoomSize = 15;

    private string WaveName;

    // Update is called once per frame
    private void OnGUI()
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

        if ((Event.current.type == EventType.MouseDrag && Event.current.button == 0) || (Event.current.type == EventType.MouseDown && Event.current.button == 0))
        {
            addNewVoxel(Mathf.FloorToInt(Event.current.mousePosition.x * viewAspect - viewPosition.x), Mathf.FloorToInt((position.height - Event.current.mousePosition.y) * viewAspect + viewPosition.y), zIndex, selectedType);
            UpdateBackgroundTexture();
        }

        if ((Event.current.type == EventType.MouseDrag && Event.current.button == 1) || (Event.current.type == EventType.MouseDown && Event.current.button == 1))
        {
            RemoveVoxel(Mathf.FloorToInt(Event.current.mousePosition.x * viewAspect - viewPosition.x), Mathf.FloorToInt((position.height - Event.current.mousePosition.y) * viewAspect + viewPosition.y), zIndex);
            UpdateBackgroundTexture();
        }

        if (backgroundTexture == null) UpdateBackgroundTexture();

        #endregion

        //draws backgound texture
        EditorGUI.DrawTextureTransparent(new Rect(0, 0, getSideBarX(), position.height), backgroundTexture);
    }

    #region internalFunctions

    private void UpdateBackgroundTexture()
    {
        if (backgroundTexture == null)
        {
            backgroundTexture = new Texture2D(Mathf.FloorToInt(getSideBarX() * viewAspect), Mathf.FloorToInt(position.height * viewAspect), TextureFormat.ARGB32, false);
            backgroundTexture.filterMode = FilterMode.Point;
            backgroundTexture.anisoLevel = 0;
        }
        else
        {
            backgroundTexture.Reinitialize(Mathf.FloorToInt(getSideBarX() * viewAspect), Mathf.FloorToInt(position.height * viewAspect));
        }

        for (var x = 0; x < backgroundTexture.width; x++)
        for (var y = 0; y < backgroundTexture.height; y++)
            if (Inview(x - (int)viewPosition.x, y - (int)viewPosition.y))
                backgroundTexture.SetPixel(x, y, getVoxelColor(x - (int)viewPosition.x, y + (int)viewPosition.y, zIndex));

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
    private bool mouseInMiddleWindow()
    {
        Vector2 mousepos = Input.mousePosition;
        if (mousepos.x < position.xMax && mousepos.x > position.xMin)
            if (mousepos.y < position.yMax && mousepos.y > position.yMin)
                return true;

        return false;
    }

    private bool CanMoveView()
    {
        return false;
    }

    private bool Inview(int x, int z)
    {
        if (x > viewPosition.x + sizeX || x < viewPosition.x)
            return false;
        if (z > viewPosition.y + sizeY || z < viewPosition.y)
            return false;
        return true;
    }

    private float getSideBarX()
    {
        return position.width - sideBarWidth;
    }

    private void addNewVoxel(int x, int y, int z, EnemyStats.Type Type)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();
        if (!Inview(x, z))
            return;

        var c = Convert.ToChar((int)Type);

        var v = new Vector3(x, y, z);

        if (!currentLevel.ContainsKey(v))
            currentLevel.Add(v, c);
        else if (currentLevel.ContainsKey(v)) currentLevel[v] = c;
    }

    private void RemoveVoxel(int x, int y, int z)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();
        if (!Inview(x, z))
            return;

        var v = new Vector3(x, y, z);
        if (currentLevel.ContainsKey(v)) currentLevel.Remove(v);
    }

    //Size is in voxels
    private bool drawFieldSize(float y)
    {
        var s = new Vector2(sizeX, sizeY);
        var z = zIndex;
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

    private Color getVoxelColor(int x, int y, int z)
    {
        if (currentLevel == null)
            currentLevel = new Dictionary<Vector3, char>();

        var v = new Vector3(x, y, z);

        if (currentLevel.ContainsKey(v))
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

        return Color.gray;
    }

    private void Save()
    {
        var hz = 0;
        foreach (var v in currentLevel.Keys)
            if (v.z > hz)
                hz = (int)v.z;
        Debug.Log(hz);
        hz++;

        float h = -1;
        foreach (var k in currentLevel)
            if (k.Key.z > h)
                h = k.Key.z;

        var SaveData = new WaveClass(currentLevel, new Vector3(sizeX, sizeY, (int)h + 1));

        Serialization.Save("Wave1", Serialization.FileTypes.Wave, SaveData);
    }

    private void Load()
    {
        var loadedData = new WaveClass();

        if (!Serialization.Load("Wave1", Serialization.FileTypes.Wave, ref loadedData))
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
