using Player;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UpgradeSystem;
using Util.DebugHelpers;
using Util.Saving;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private PlayerStats playerStats;
        [SerializeField] private PlayerWeaponControler playerWeaponControler;
        [SerializeField] private PickupManager pickupManager;
        [ShowInInspector] private UpgradeData _upgrades;

        public static PlayerStats Stats => _instance.playerStats;
        public static PlayerWeaponControler WeaponController => _instance.playerWeaponControler;
        public static PickupManager PickupManager => _instance.pickupManager;

        public static UpgradeData Upgrades
        {
            get
            {
                if (!Instance)
                {
                    return new UpgradeData();
                }

                if (Instance._upgrades == null)
                {
                    Serialization.Load("upgrade", Serialization.FileTypes.Binary, out Instance._upgrades);
                }

                return Instance._upgrades;
            }
        }

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

            Serialization.Load("upgrade", Serialization.FileTypes.Binary, out _upgrades); //TODO move to a playerSaveData manager of some sort

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
