using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ValueClasses;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [FormerlySerializedAs("Energy")] [SerializeField] private Bar energy;
        [FormerlySerializedAs("Health")] [SerializeField] private Bar health;
        [FormerlySerializedAs("CollectedScrapDisplay")] [SerializeField] private Text collectedScrapDisplay;

        public float currentEnergy;
        public float maxEnergy = 200;

        public float currentHealth;
        public float maxHealth = 100;

        public float currentShield;
        public float maxShield;

        private void Start()
        {
            maxHealth = (GameManager.Upgrades.hullUpgradeLevel + 1) * 100;
            maxShield = GameManager.Upgrades.shieldCapacitorLevel * 100;
            energy.Init(maxEnergy, 2f);
            health.Init(maxHealth, 100);

            currentHealth = maxHealth;
            currentEnergy = maxEnergy;
            health.UpdateSize(maxHealth);
        }

        // Update is called once per frame
        private void Update()
        {
            if (currentEnergy < maxEnergy && !PlayerWeaponControler.Firing)
                currentEnergy += 16f * Time.deltaTime;

            //Update Status display
            energy.UpdateSize(currentEnergy);
            health.UpdateSize(currentHealth);
            collectedScrapDisplay.text = "Scrap Collected: " + GameManager.PickupManager.PickedUpScrap;
        }

        public bool CanFire(float energyNeeded)
        {
            if (currentEnergy >= energyNeeded)
                return true;
            return false;
        }

        public void Hit(float value)
        {
            currentHealth -= value;
        }

        public void RemoveEnergy(float amount)
        {
            currentEnergy -= amount;
            if (currentEnergy < 0)
                currentEnergy = 0;
        }
    }
}
