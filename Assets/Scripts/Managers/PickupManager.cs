using Entities.Generic;
using Pools;
using UnityEngine;

namespace Managers
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickupManager : MonoBehaviour
    {
        private int _pickupLayer;
        public float PickedUpScrap { get; private set; }

        // Use this for initialization
        private void Start()
        {
            _pickupLayer = LayerMask.NameToLayer("Pickup");
            GetComponent<SphereCollider>().radius = 7 * Mathf.Pow(1.2f, SaveDataManager.Upgrades.ScrapCollectionRange);
        }

        private void OnDestroy()
        {
            var dat = SaveDataManager.Upgrades;
            SaveDataManager.Upgrades.upgradeCurrency += Mathf.FloorToInt(PickedUpScrap * ((dat.ScrapConversionRate + 1) * 1.1f));
        }

        public void OnTriggerStay(Collider col)
        {
            if (col.gameObject.layer == _pickupLayer)
            {
                var rigidBody = col.GetComponent<Rigidbody>();
                if (!rigidBody) return;

                var speed = rigidBody.linearVelocity.magnitude;
                if (speed < 0) speed *= -1;

                if (speed < 20)
                {
                    Vector2 v2 = transform.position - col.transform.position;
                    v2.Normalize();
                    rigidBody.AddForce(v2 * (20f * Mathf.Pow(1.5f, SaveDataManager.Upgrades.ScrapCollectionSpeed)) * Time.deltaTime);
                }
            }
        }

        public void OnTriggerExit(Collider col)
        {
            if (col.gameObject.layer == _pickupLayer) col.GetComponent<Rigidbody>().linearDamping = 5f;
        }

        public void OnTriggerEnter(Collider col)
        {
            if (col.gameObject.layer == _pickupLayer) col.GetComponent<Rigidbody>().linearDamping = 1f;
        }

        public void OnCollisionEnter(Collision col)
        {
            if (col.gameObject.layer == _pickupLayer)
            {
                ScrapPickupPool.RemovePickup(col.gameObject.GetComponent<ScrapPickup>());
                PickedUpScrap += col.gameObject.GetComponent<ScrapPickup>().ScrapValue;
            }
        }
    }
}
