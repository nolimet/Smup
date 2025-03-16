using ObjectPools;
using Pickups;
using UnityEngine;
using UpgradeSystem;
using Util;
using Util.Saving;

namespace Managers
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class PickupManager : MonoBehaviour
    {
        private int _pickupLayer;
        public float PickedUpScrap { get; private set; }

        // Use this for initialization
        private void Start()
        {
            _pickupLayer = LayerMask.NameToLayer("Pickup");
            GetComponent<CircleCollider2D>().radius = 7 * Mathf.Pow(1.2f, GameManager.Upgrades.scrapCollectionRange);
        }

        private void OnDestroy()
        {
            var dat = new UpgradeData();
            Serialization.Load("upgrade", Serialization.FileTypes.Binary, ref dat); //TODO move to saveDataManager

            dat.upgradeCurrency += Mathf.FloorToInt(PickedUpScrap * ((dat.scrapConversionRate + 1) * 1.1f));

            Serialization.Save("upgrade", Serialization.FileTypes.Binary, dat); //TODO move to saveDataManager
        }

        public void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.layer == _pickupLayer)
            {
                var rigidBody = col.GetComponent<Rigidbody2D>();
                if (!rigidBody)
                    return;

                var speed = rigidBody.linearVelocity.GetLength();
                if (speed < 0) speed *= -1;

                if (speed < 20)
                {
                    Vector2 v2 = transform.position - col.transform.position;
                    v2.Normalize();
                    rigidBody.AddForce(v2 * (20f * Mathf.Pow(1.5f, GameManager.Upgrades.scrapCollectionSpeed)) * Time.deltaTime);
                }
            }
        }

        public void OnTriggerExit2D(Collider2D col)
        {
            if (col.gameObject.layer == _pickupLayer) col.GetComponent<Rigidbody2D>().linearDamping = 5f;
        }

        public void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.layer == _pickupLayer) col.GetComponent<Rigidbody2D>().linearDamping = 1f;
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (col.gameObject.layer == _pickupLayer)
            {
                ScrapPickupPool.RemovePickup(col.gameObject.GetComponent<ScrapPickup>());
                PickedUpScrap += col.gameObject.GetComponent<ScrapPickup>().ScrapValue;
            }
        }
    }
}
