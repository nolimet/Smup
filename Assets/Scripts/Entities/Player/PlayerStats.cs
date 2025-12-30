using Interfaces;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using ValueClasses;

namespace Player
{
	public class PlayerStats : MonoBehaviour, IDamageAble
	{
		[FormerlySerializedAs("Energy")] [SerializeField] private Bar energy;
		[FormerlySerializedAs("Health")] [SerializeField] private Bar health;
		[FormerlySerializedAs("CollectedScrapDisplay")] [SerializeField] private Text collectedScrapDisplay;

		public double currentEnergy;
		public double maxEnergy = 200;

		public double currentHealth;
		public double maxHealth = 100;

		public double currentShield;
		public double maxShield;

		private void Start()
		{
			maxHealth = (SaveDataManager.Upgrades.HullUpgradeLevel + 1) * 100;
			maxShield = SaveDataManager.Upgrades.ShieldCapacitorLevel * 100;
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
			{
				currentEnergy += 16f * Time.deltaTime;
			}

			//Update Status display
			energy.UpdateSize(currentEnergy);
			health.UpdateSize(currentHealth);
			collectedScrapDisplay.text = "Scrap Collected: " + GameManager.PickupManager.PickedUpScrap;
		}

		public bool CanFire(float energyNeeded)
		{
			if (currentEnergy >= energyNeeded)
			{
				return true;
			}
			return false;
		}

		public void ReceiveDamage(double damage)
		{
			if (currentShield > 0)
			{
				currentShield -= damage;
				if (currentShield < 0)
				{
					damage = -currentShield;
					currentHealth -= damage;
					currentShield = 0;
				}
			}

			currentHealth -= damage;
			if (currentShield < 0)
			{
				Debug.Log("TODO: Implement game-over!"); //TODO implement game over
			}
		}

		public void RemoveEnergy(float amount)
		{
			currentEnergy -= amount;
			if (currentEnergy < 0)
			{
				currentEnergy = 0;
			}
		}
	}
}
