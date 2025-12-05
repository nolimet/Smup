using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UpgradeSystem;
using Util.DebugHelpers;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		[SerializeField] private PlayerStats playerStats;
		[SerializeField] private PlayerWeaponControler playerWeaponControler;
		[SerializeField] private PickupManager pickupManager;

		// ReSharper disable once InconsistentNaming
		[ShowInInspector] private UpgradeData _upgrades => Application.isPlaying ? SaveDataManager.Upgrades : null;

		public static PlayerStats Stats => Instance.playerStats;
		public static PlayerWeaponControler WeaponController => Instance.playerWeaponControler;
		public static PickupManager PickupManager => Instance.pickupManager;

		public static GameManager Instance
		{
			get
			{
				if (_instance == null || !_instance)
				{
					FindAnyObjectByType<GameManager>();
				}

				if (_instance == null || !_instance)
				{
					Debug.LogWarning("No GameManager object found");
				}

				return _instance;
			}
		}

		private static GameManager _instance;

		public void Awake()
		{
			if (_instance != null)
			{
				Debug.LogError("More than one GameManager!. Destroying duplicate GameManager");
				Destroy(this);
				return;
			}

			_instance = this;

			Debugger.DebugEnabled = true;
		}

		public void OnDestroy()
		{
			_instance = null;
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Escape)) //TODO refactor to use a asset refrence. To make sure we returned to the menu
			{
				SceneManager.LoadScene(0, LoadSceneMode.Single);
			}
		}
	}
}
