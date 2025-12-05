using System;
using System.Collections.Generic;
using Enums;
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
	private Texture2D _backgroundTexture = null;

	private Vector2 _viewPosition = Vector2.zero;

//    Color seeThrough = new Color(0, 0, 0, 0);
	private Dictionary<Vector3, char> _currentLevel;
	private EnemyType _selectedType = EnemyType.Shooter;
	private float ViewAspect => 1f / _zoomSize;

	//editor defined varibles
	private float _sideBarWidth = 250f;
	private int _sizeX = 30, _sizeY = 20, _zIndex = 0;
	private int _zoomSize = 15;

	private string _waveName;

	// Update is called once per frame
	private void OnGUI()
	{
		//calculate background
		if (DrawFieldSize(1))
		{
			UpdateBackgroundTexture();
		}

		//set enemy type
		_selectedType = (EnemyType) EditorGUI.EnumPopup(new Rect(GetSideBarX(), 65, _sideBarWidth, 15), "Selected Enemy", _selectedType);

		#region save, load and reset
		//clear screen
		if (GUI.Button(new Rect(GetSideBarX(), 85, _sideBarWidth, 15), "Reset"))
		{
			_currentLevel = new Dictionary<Vector3, char>();
			_viewPosition = new Vector2();
			UpdateBackgroundTexture();
		}

		//save DAta
		if (GUI.Button(new Rect(GetSideBarX(), 105, _sideBarWidth, 15), "SAVE WAVE"))
		{
			Save();
		}
		//load Data
		if (GUI.Button(new Rect(GetSideBarX(), 125, _sideBarWidth, 15), "LOAD WAVE"))
		{
			Load();
		}
		#endregion

		#region Controles
		if (Event.current.type == EventType.MouseDrag && Event.current.button == 0 || Event.current.type == EventType.MouseDown && Event.current.button == 0)
		{
			AddNewVoxel(Mathf.FloorToInt(Event.current.mousePosition.x * ViewAspect - _viewPosition.x), Mathf.FloorToInt((position.height - Event.current.mousePosition.y) * ViewAspect + _viewPosition.y), _zIndex, _selectedType);
			UpdateBackgroundTexture();
		}

		if (Event.current.type == EventType.MouseDrag && Event.current.button == 1 || Event.current.type == EventType.MouseDown && Event.current.button == 1)
		{
			RemoveVoxel(Mathf.FloorToInt(Event.current.mousePosition.x * ViewAspect - _viewPosition.x), Mathf.FloorToInt((position.height - Event.current.mousePosition.y) * ViewAspect + _viewPosition.y), _zIndex);
			UpdateBackgroundTexture();
		}

		if (_backgroundTexture == null)
		{
			UpdateBackgroundTexture();
		}
		#endregion

		//draws backgound texture
		EditorGUI.DrawTextureTransparent(new Rect(0, 0, GetSideBarX(), position.height), _backgroundTexture);
	}

	#region internalFunctions
	private void UpdateBackgroundTexture()
	{
		if (_backgroundTexture == null)
		{
			_backgroundTexture = new Texture2D(Mathf.FloorToInt(GetSideBarX() * ViewAspect), Mathf.FloorToInt(position.height * ViewAspect), TextureFormat.ARGB32, false);
			_backgroundTexture.filterMode = FilterMode.Point;
			_backgroundTexture.anisoLevel = 0;
		}
		else
		{
			_backgroundTexture.Reinitialize(Mathf.FloorToInt(GetSideBarX() * ViewAspect), Mathf.FloorToInt(position.height * ViewAspect));
		}

		for (var x = 0; x < _backgroundTexture.width; x++)
		for (var y = 0; y < _backgroundTexture.height; y++)
			if (Inview(x - (int) _viewPosition.x, y - (int) _viewPosition.y))
			{
				_backgroundTexture.SetPixel(x, y, GetVoxelColor(x - (int) _viewPosition.x, y + (int) _viewPosition.y, _zIndex));
			}

		_backgroundTexture.Apply();
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
	private bool MouseInMiddleWindow()
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

	private bool CanMoveView() => false;

	private bool Inview(int x, int z)
	{
		if (x > _viewPosition.x + _sizeX || x < _viewPosition.x)
		{
			return false;
		}
		if (z > _viewPosition.y + _sizeY || z < _viewPosition.y)
		{
			return false;
		}
		return true;
	}

	private float GetSideBarX() => position.width - _sideBarWidth;

	private void AddNewVoxel(int x, int y, int z, EnemyType type)
	{
		_currentLevel ??= new Dictionary<Vector3, char>();
		if (!Inview(x, z))
		{
			return;
		}

		var c = Convert.ToChar((int) type);

		var v = new Vector3(x, y, z);

		_currentLevel[v] = c;
	}

	private void RemoveVoxel(int x, int y, int z)
	{
		if (_currentLevel == null)
		{
			_currentLevel = new Dictionary<Vector3, char>();
		}
		if (!Inview(x, z))
		{
			return;
		}

		var v = new Vector3(x, y, z);
		_currentLevel.Remove(v);
	}

	//Size is in voxels
	private bool DrawFieldSize(float y)
	{
		var s = new Vector2(_sizeX, _sizeY);
		var z = _zIndex;
		s = EditorGUI.Vector2Field(new Rect(GetSideBarX(), y, _sideBarWidth, 20), "Size", s);
		z = EditorGUI.IntField(new Rect(GetSideBarX(), 45, _sideBarWidth, 15), "zIndex", z);

		if (!Mathf.Approximately(s.y, _sizeY) || !Mathf.Approximately(s.x, _sizeX) || z != _zIndex)
		{
			if (s.y > 1)
			{
				_sizeY = (int) s.y;
			}
			else
			{
				_sizeY = 1;
			}
			if (s.x > 1)
			{
				_sizeX = (int) s.x;
			}
			else
			{
				_sizeX = 1;
			}
			if (z > 0)
			{
				_zIndex = z;
			}
			else
			{
				_zIndex = 0;
			}
			return true;
		}

		return false;
	}

	private Color GetVoxelColor(int x, int y, int z)
	{
		if (_currentLevel == null)
		{
			_currentLevel = new Dictionary<Vector3, char>();
		}

		var v = new Vector3(x, y, z);

		if (_currentLevel.ContainsKey(v))
		{
			switch (_currentLevel[v])
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

	private void Save()
	{
		var hz = 0;
		foreach (var v in _currentLevel.Keys)
			if (v.z > hz)
			{
				hz = (int) v.z;
			}
		Debug.Log(hz);
		hz++;

		float h = -1;
		foreach (var k in _currentLevel)
			if (k.Key.z > h)
			{
				h = k.Key.z;
			}

		var saveData = new WaveClass(_currentLevel, new Vector3(_sizeX, _sizeY, (int) h + 1));

		Serialization.SaveWave("Wave1", saveData);
	}

	private void Load()
	{
		if (!Serialization.TryLoadWave("Wave1", out var loadedData))
		{
			_currentLevel = new Dictionary<Vector3, char>();
			UpdateBackgroundTexture();
			return;
		}

		Debug.Log("Wave points: " + loadedData.waves.Length);
		_currentLevel = loadedData.Convert();
		UpdateBackgroundTexture();
	}
	#endregion
	#endregion
}
