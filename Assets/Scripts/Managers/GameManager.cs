using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UpgradeSystem;
using Util;
using Util.DebugHelpers;
using Util.Saving;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [FormerlySerializedAs("_screen")] [SerializeField] private MoveBoxScaler screen;
        [FormerlySerializedAs("_playerStats")] [SerializeField] private PlayerStats playerStats;
        [FormerlySerializedAs("_playerWeaponControler")] [SerializeField] private PlayerWeaponControler playerWeaponControler;
        [FormerlySerializedAs("_upgrades")] [SerializeField] private UpgradeData upgrades;
        [FormerlySerializedAs("_pickupManager")] [SerializeField] private PickupManager pickupManager;

        public static MoveBoxScaler Screen;
        public static PlayerStats Stats;
        public static PlayerWeaponControler WeaponController;

        public static UpgradeData Upgrades
        {
            get
            {
                if (Instance.upgrades == null) Serialization.Load("upgrade", Serialization.FileTypes.Binary, ref Instance.upgrades);

                return Instance.upgrades;
            }
        }

        public static PickupManager PickupManager;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null || !_instance)
                    FindAnyObjectByType<GameManager>();

                if (_instance == null || !_instance)
                    Debug.Log("INSTANCE NOT FOUND");

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

            Screen = screen;
            Stats = playerStats;
            WeaponController = playerWeaponControler;
            PickupManager = pickupManager;

            Serialization.Load("upgrade", Serialization.FileTypes.Binary, ref upgrades); //TODO move to a playerSaveData manager of some sort

            Debugger.DebugEnabled = true;
        }

        public void OnDestroy()
        {
            Screen = null;
            Stats = null;
            WeaponController = null;
            PickupManager = null;
            _instance = null;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape)) //TODO refactor to use a asset refrence. To make sure we returned to the menu
                SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
